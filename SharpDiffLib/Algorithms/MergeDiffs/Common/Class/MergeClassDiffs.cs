using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Implementation;
using KST.SharpDiffLib.Internal.Members;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Common.Class
{
	internal class MergeClassDiffs<TType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<IDiff<TType>, IDiff<TType>, IConflictContainer, List<IDiffItem>> aCompiled;

		public MergeClassDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			if (this.aCompiled == null)
			{
				var compiled = this.Compile();

				this.aCompiled = compiled.Compile();
			}

			var ret = this.aCompiled(left, right, conflicts);

			return new Diff<TType>(ret);

		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private Expression<Func<IDiff<TType>, IDiff<TType>, IConflictContainer, List<IDiffItem>>> Compile()
		{
			ParameterExpression left = Expression.Parameter(typeof(IDiff<TType>), "left");
			ParameterExpression right = Expression.Parameter(typeof(IDiff<TType>), "right");
			ParameterExpression conflicts = Expression.Parameter(typeof(IConflictContainer), "ret");
			ParameterExpression ret = Expression.Parameter(typeof(List<IDiffItem>), "ret");
			ParameterExpression leftEnumerator = Expression.Parameter(typeof(IEnumerator<IDiffItem>), "leftEnumerator");
			ParameterExpression leftSomeLeft = Expression.Parameter(typeof(bool), "leftSomeLeft");
			ParameterExpression rightEnumerator = Expression.Parameter(typeof(IEnumerator<IDiffItem>), "rightEnumerator");
			ParameterExpression rightSomeLeft = Expression.Parameter(typeof(bool), "rightSomeLeft");

			return Expression.Lambda<Func<IDiff<TType>, IDiff<TType>, IConflictContainer, List<IDiffItem>>>(
				Expression.Block(
					new[] { ret, leftEnumerator, leftSomeLeft, rightEnumerator, rightSomeLeft },
					Expression.Assign(
						ret,
						Expression.New(
							ListMembers.NewWithCount(typeof(IDiffItem)),
							Expression.Add(
								Expression.Property(left, DiffMembers.Count()),
								Expression.Property(right, DiffMembers.Count())
							)
						)
					),
					Expression.Assign(
						leftEnumerator,
						Expression.Call(left, EnumerableMembers.GetEnumerator(typeof(IDiffItem)))
					),
					Expression.Assign(
						rightEnumerator,
						Expression.Call(right, EnumerableMembers.GetEnumerator(typeof(IDiffItem)))
					),
					this.MoveEnumerator(leftEnumerator, leftSomeLeft),
					this.MoveEnumerator(rightEnumerator, rightSomeLeft),
					Expression.Block(Class<TType>.Properties.Select(x => this.EvaluateProperty(leftEnumerator, leftSomeLeft, rightEnumerator, rightSomeLeft, conflicts, ret, x))),
					ret
				),
				left, right, conflicts
			);
		}

		private Expression EvaluateProperty(ParameterExpression leftEnumerator, ParameterExpression leftSomeLeft, ParameterExpression rightEnumerator, ParameterExpression rightSomeLeft, Expression conflicts, Expression ret, Property property)
		{
			Expression leftCurrent = Expression.Property(leftEnumerator, EnumerableMembers.Current(typeof(IDiffItem)));
			Expression rightCurrent = Expression.Property(rightEnumerator, EnumerableMembers.Current(typeof(IDiffItem)));

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
							DiffItemsMembers.ClassProperty()
						),
						FastPropertyMembers.UniqueID()
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
							DiffItemsMembers.ClassProperty()
						),
						FastPropertyMembers.UniqueID()
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
							this.CompileCheckConflicts(property, leftCurrent, rightCurrent, conflicts, ret),
							this.MoveEnumerator(rightEnumerator, rightSomeLeft)
						),
						Expression.Call(
							ret,
							ListMembers.Add(typeof(IDiffItem)),
							leftCurrent
						)
					),
					this.MoveEnumerator(leftEnumerator, leftSomeLeft)
				),
				Expression.IfThen(
					rightShouldBeProcessed,
					Expression.Block(
						Expression.Call(
							ret,
							ListMembers.Add(typeof(IDiffItem)),
							rightCurrent
						),
						this.MoveEnumerator(rightEnumerator, rightSomeLeft)
					)
				)
			);
		}

		private Expression CompileCheckConflicts(Property property, Expression leftCurrent, Expression rightCurrent, Expression conflicts, Expression ret)
		{
			/* PSEUDO CODE FOR THIS:
			 * if (leftCurrent is Unchanged)
			 * {
			 *     add(rightCurrent)
			 * }
			 * else if (rightCurrent is Unchanged)
			 * {
			 *     add(leftCurrent)
			 * }
			 * else if (leftCurrent is Replace || rightCurrent is Replace)
			 * {
			 *     if (leftCurrent == rightCurrent)
			 *          add(leftCurrent)
			 *     else
			 *     {
			 *          conflict = Conflict(leftCurrent, rightCurrent)
			 *          add(conflict)
			 *          conflicts.register(conflict)
			 *     }
			 * }
			 * else if (leftCurrent is Changed && rightCurrent is Changed)
			 * {
			 *     add(algorithm.Merge(leftCurrent.ValueDiff, rightCurrent.ValueDiff, conflicts))
			 * }
			 * else
			 *     throw
			 */

			IMergeDiffsAlgorithm algorithm = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm(property.Type);
			Type itemReplacedType = typeof(IDiffItemReplaced<>).MakeGenericType(property.Type);

			ParameterExpression conflict = Expression.Parameter(typeof(IDiffItemConflicted), "conflict");

			return Expression.IfThenElse(
				Expression.TypeIs(leftCurrent, typeof(IDiffItemUnchanged)),
				Expression.Call(
					ret,
					ListMembers.Add(typeof(IDiffItem)),
					rightCurrent
				),
				Expression.IfThenElse(
					Expression.TypeIs(rightCurrent, typeof(IDiffItemUnchanged)),
					Expression.Call(
						ret,
						ListMembers.Add(typeof(IDiffItem)),
						leftCurrent
					),
					Expression.IfThenElse(
						Expression.OrElse(
							Expression.TypeIs(leftCurrent, itemReplacedType),
							Expression.TypeIs(rightCurrent, itemReplacedType)
						),
						Expression.IfThenElse(
							Expression.Call(
								null,
								ObjectMembers.Equals(),
								leftCurrent,
								rightCurrent
							),
							Expression.Call(
								ret,
								ListMembers.Add(typeof(IDiffItem)),
								leftCurrent
							),
							Expression.Block(
								new[] { conflict },
								Expression.Assign(
									conflict,
									Expression.New(
										DiffItemsMembers.NewConflict(),
										leftCurrent,
										rightCurrent
									)
								),
								Expression.Call(
									ret,
									ListMembers.Add(typeof(IDiffItem)),
									conflict
								),
								Expression.Call(
									conflicts,
									ConflictContainerMembers.RegisterConflict(),
									conflict
								)
							)
						),
						Expression.IfThenElse(
							Expression.AndAlso(
								Expression.TypeIs(leftCurrent, typeof(IDiffItemChanged)),
								Expression.TypeIs(rightCurrent, typeof(IDiffItemChanged))
							),
							Expression.Call(
								ret,
								ListMembers.Add(typeof(IDiffItem)),
								Expression.New(
									DiffItemsMembers.NewClassChanged(property.Type),
									Expression.Constant(property),
									Expression.Call(
										Expression.Constant(algorithm, typeof(IMergeDiffsAlgorithm<>).MakeGenericType(property.Type)),
										MergeDiffsAlgorithmMembers.MergeDiffs(property.Type),
										Expression.Property(
											Expression.Convert(leftCurrent, typeof(IDiffItemChanged<>).MakeGenericType(property.Type)),
											DiffItemsMembers.ChangedDiff(property.Type)
										),
										Expression.Property(
											Expression.Convert(rightCurrent, typeof(IDiffItemChanged<>).MakeGenericType(property.Type)),
											DiffItemsMembers.ChangedDiff(property.Type)
										),
										conflicts
									)
								)
							),
					
							Expression.Throw(
								Expression.New(typeof(Exception))
							)
						)
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
					EnumerableMembers.MoveNext(typeof(IDiffItem))
				)
			);
		}
	}
}
