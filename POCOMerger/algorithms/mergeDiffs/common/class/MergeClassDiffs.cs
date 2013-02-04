using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.mergeDiffs.common.@class
{
	internal class MergeClassDiffs<TType> : IMergeDiffsAlgorithm<TType>
	{
		private struct CompiledReturn
		{
#pragma warning disable 649
			public List<IDiffItem> aDiff;
			public bool aHadConflicts;
#pragma warning restore 649
		}

		private readonly MergerImplementation aMergerImplementation;
		private Func<IDiff<TType>, IDiff<TType>, CompiledReturn> aCompiled;

		public MergeClassDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			if (this.aCompiled == null)
			{
				var compiled = this.Compile();

				this.aCompiled = compiled.Compile();
			}

			var ret = this.aCompiled(left, right);

			hadConflicts = ret.aHadConflicts;
			return new Diff<TType>(ret.aDiff);

		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, out bool hadConflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, out hadConflicts);
		}

		#endregion

		private Expression<Func<IDiff<TType>, IDiff<TType>, CompiledReturn>> Compile()
		{
			ParameterExpression left = Expression.Parameter(typeof(IDiff<TType>), "left");
			ParameterExpression right = Expression.Parameter(typeof(IDiff<TType>), "right");
			ParameterExpression ret = Expression.Parameter(typeof(CompiledReturn), "ret");
			Expression diff = Expression.Field(ret, "aDiff");
			Expression hadConflicts = Expression.Field(ret, "aHadConflicts");
			ParameterExpression leftEnumerator = Expression.Parameter(typeof(IEnumerator<IDiffItem>), "leftEnumerator");
			ParameterExpression leftSomeLeft = Expression.Parameter(typeof(bool), "leftSomeLeft");
			ParameterExpression rightEnumerator = Expression.Parameter(typeof(IEnumerator<IDiffItem>), "rightEnumerator");
			ParameterExpression rightSomeLeft = Expression.Parameter(typeof(bool), "rightSomeLeft");

			return Expression.Lambda<Func<IDiff<TType>, IDiff<TType>, CompiledReturn>>(
				Expression.Block(
					new[] { ret, leftEnumerator, leftSomeLeft, rightEnumerator, rightSomeLeft },
					Expression.Assign(
						diff,
						Expression.New(
							Members.List.NewWithCount(typeof(IDiffItem)),
							Expression.Add(
								Expression.Property(left, Members.Diff.Count()),
								Expression.Property(right, Members.Diff.Count())
							)
						)
					),
					Expression.Assign(hadConflicts, Expression.Constant(false)),
					Expression.Assign(
						leftEnumerator,
						Expression.Call(left, Members.Enumerable.GetEnumerator(typeof(IDiffItem)))
					),
					Expression.Assign(
						rightEnumerator,
						Expression.Call(right, Members.Enumerable.GetEnumerator(typeof(IDiffItem)))
					),
					this.MoveEnumerator(leftEnumerator, leftSomeLeft),
					this.MoveEnumerator(rightEnumerator, rightSomeLeft),
					Expression.Block(Class<TType>.Properties.Select(x => this.EvaluateProperty(leftEnumerator, leftSomeLeft, rightEnumerator, rightSomeLeft, hadConflicts, diff, x))),
					ret
				),
				left, right
			);
		}

		private Expression EvaluateProperty(ParameterExpression leftEnumerator, ParameterExpression leftSomeLeft, ParameterExpression rightEnumerator, ParameterExpression rightSomeLeft, Expression hadConflicts, Expression diff, Property property)
		{
			Expression leftCurrent = Expression.Property(leftEnumerator, Members.Enumerable.Current(typeof(IDiffItem)));
			Expression rightCurrent = Expression.Property(rightEnumerator, Members.Enumerable.Current(typeof(IDiffItem)));

			/* PSEUDO CODE FOR THIS:
			 * if (leftSomeLeft && leftCurrent.UniqueID == ID)
			 * {
			 *     if (rightSomeLeft && rightCurrent.UniqueID == ID)
			 *     {
			 *         CheckConflicts(leftCurrent, rightCurrent)
			 *         rightSomeLeft = next(rightCurrent)
			 *     }
			 *     else
			 *         add(leftCurrent)
			 *     leftSomeLeft = next(leftCurrent)
			 * }
			 * else if (rightSomeLeft && rightCurrent.UniqueID == ID)
			 * {
			 *     add(rightCurrent)
			 *     rightSomeLeft = next(rightCurrent)
			 * }
			 */

			Expression leftShouldBeProcessed = Expression.AndAlso(
				leftSomeLeft,
				Expression.Equal(
					Expression.Property(
						Expression.Property(
							Expression.Convert(
								leftCurrent,
								typeof(IDiffClassItem)
							),
							Members.DiffItems.ClassProperty()
						),
						Members.FastProperty.UniqueID()
					),
					Expression.Constant(property.UniqueID)
				)
			);

			Expression rightShouldBeProcessed = Expression.AndAlso(
				rightSomeLeft,
				Expression.Equal(
					Expression.Property(
						Expression.Property(
							Expression.Convert(
								rightCurrent,
								typeof(IDiffClassItem)
							),
							Members.DiffItems.ClassProperty()
						),
						Members.FastProperty.UniqueID()
					),
					Expression.Constant(property.UniqueID)
				)
			);

			return Expression.IfThenElse(
				leftShouldBeProcessed,
				Expression.Block(
					Expression.IfThenElse(
						rightShouldBeProcessed,
						Expression.Block(
							this.CompileCheckConflicts(property, leftCurrent, rightCurrent, hadConflicts, diff),
							this.MoveEnumerator(rightEnumerator, rightSomeLeft)
						),
						Expression.Call(
							diff,
							Members.List.Add(typeof(IDiffItem)),
							leftCurrent
						)
					),
					this.MoveEnumerator(leftEnumerator, leftSomeLeft)
				),
				Expression.IfThen(
					rightShouldBeProcessed,
					Expression.Block(
						Expression.Call(
							diff,
							Members.List.Add(typeof(IDiffItem)),
							rightCurrent
						)
					)
				)
			);
		}

		private Expression CompileCheckConflicts(Property property, Expression leftCurrent, Expression rightCurrent, Expression hadConflicts, Expression diff)
		{
			/* PSEUDO CODE FOR THIS:
			 * if (leftCurrent is Replace || rightCurrent is Replace)
			 * {
			 *     if (leftCurrent == rightCurrent)
			 *          add(leftCurrent)
			 *     else
			 *     {
			 *          add(Conflict(leftCurrent, rightCurrent))
			 *          hadConflicts = true
			 *     }
			 * }
			 * else if (leftCurrent is Changed && rightCurrent is Changed)
			 * {
			 *     add(algorithm.Merge(leftCurrent.ValueDiff, rightCurrent.ValueDiff, out hC))
			 *     if (hC) hasConflicts = true
			 * }
			 * else
			 *     throw
			 */

			IMergeDiffsAlgorithm algorithm = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm(property.Type);
			Type itemReplacedType = typeof(IDiffItemReplaced<>).MakeGenericType(property.Type);

			ParameterExpression hadConflictsInternal = Expression.Parameter(typeof(bool), "hadConflictsInternal");

			return Expression.IfThenElse(
				Expression.OrElse(
					Expression.TypeIs(leftCurrent, itemReplacedType),
					Expression.TypeIs(rightCurrent, itemReplacedType)
				),
				Expression.IfThenElse(
					Expression.Call(
						null,
						Members.Object.Equals(),
						leftCurrent,
						rightCurrent
					),
					Expression.Call(
						diff,
						Members.List.Add(typeof(IDiffItem)),
						leftCurrent
					),
					Expression.Block(
						Expression.Call(
							diff,
							Members.List.Add(typeof(IDiffItem)),
							Expression.New(
								Members.DiffItems.NewConflict(),
								leftCurrent,
								rightCurrent
							)
						),
						Expression.Assign(
							hadConflicts,
							Expression.Constant(true)
						)
					)
				),
				Expression.IfThenElse(
					Expression.AndAlso(
						Expression.TypeIs(leftCurrent, typeof(IDiffItemChanged)),
						Expression.TypeIs(rightCurrent, typeof(IDiffItemChanged))
					),
					Expression.Block(
						new[] { hadConflictsInternal },
						Expression.Call(
							diff,
							Members.List.Add(typeof(IDiffItem)),
							Expression.New(
								Members.DiffItems.NewClassChanged(property.Type),
								Expression.Constant(property),
								Expression.Call(
									Expression.Constant(algorithm, typeof(IMergeDiffsAlgorithm<>).MakeGenericType(property.Type)),
									Members.MergeDiffsAlgorithm.MergeDiffs(property.Type),
									Expression.Property(
										Expression.Convert(leftCurrent, typeof(IDiffItemChanged<>).MakeGenericType(property.Type)),
										Members.DiffItems.ChangedDiff(property.Type)
									),
									Expression.Property(
										Expression.Convert(rightCurrent, typeof(IDiffItemChanged<>).MakeGenericType(property.Type)),
										Members.DiffItems.ChangedDiff(property.Type)
									),
									hadConflictsInternal
								)
							)
						),
						Expression.IfThen(
							hadConflictsInternal,
							Expression.Assign(
								hadConflicts,
								Expression.Constant(true)
							)
						)
					),
					Expression.Throw(
						Expression.New(typeof(Exception))
					)
				)
			);
		}

		private Expression MoveEnumerator(ParameterExpression enumerator, ParameterExpression someLeft)
		{
			return Expression.Assign(
				someLeft,
				Expression.Call(
					enumerator,
					Members.Enumerable.MoveNext(typeof(IDiffItem))
				)
			);
		}
	}
}
