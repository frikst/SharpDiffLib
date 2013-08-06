using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.type;
using SharpDiffLib.fastReflection;
using SharpDiffLib.implementation;
using SharpDiffLib.@internal;

namespace SharpDiffLib.algorithms.applyPatch.common.@class
{
	internal class ApplyClassPatch<TType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<TType, IDiff, TType> aCompiled;

		public ApplyClassPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aCompiled = null;
		}

		#region Implementation of IApplyPatchAlgorithm

		public object Apply(object source, IDiff patch)
		{
			return this.Apply((TType)source, (IDiff<TType>)patch);
		}

		#endregion

		#region Implementation of IApplyPatchAlgorithm<TType>

		public TType Apply(TType source, IDiff<TType> patch)
		{
			if (this.aCompiled == null)
			{
				var compiled = this.Compile();

				this.aCompiled = compiled.Compile();
			}

			return this.aCompiled(source, patch);
		}

		#endregion

		private Expression<Func<TType, IDiff, TType>> Compile()
		{
			ParameterExpression orig = Expression.Parameter(typeof(TType), "orig");
			ParameterExpression diff = Expression.Parameter(typeof(IDiff), "diff");
			ParameterExpression ret = Expression.Parameter(typeof(TType), "ret");
			ParameterExpression enumerator = Expression.Parameter(typeof(IEnumerator<IDiffItem>), "enumerator");
			ParameterExpression someLeft = Expression.Parameter(typeof(bool), "someLeft");

			return Expression.Lambda<Func<TType, IDiff, TType>>(
				Expression.Block(
					new[] { ret, enumerator, someLeft },
					Expression.Assign(
						ret,
						Expression.New(typeof(TType))
					),
					Expression.Assign(
						enumerator,
						Expression.Call(diff, Members.Enumerable.GetEnumerator(typeof(IDiffItem)))
					),
					this.MoveEnumerator(enumerator, someLeft),
					Expression.Block(Class<TType>.Properties.Select(x => this.EvaluateProperty(enumerator, ret, orig, someLeft, x))),
					ret
				),
				orig, diff
			);
		}

		private Expression EvaluateProperty(ParameterExpression enumerator, ParameterExpression ret, ParameterExpression orig, ParameterExpression someLeft, Property property)
		{
			ParameterExpression item = Expression.Parameter(typeof(IDiffClassItem), "item");

			IApplyPatchAlgorithm algorithm = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm(property.Type);

			Expression applicator = Expression.IfThenElse(
				Expression.TypeIs(item, typeof(IDiffItemReplaced)),
				Expression.Assign(
					Expression.Property(ret, property.ReflectionPropertyInfo),
					Expression.Property(
						Expression.Convert(item, typeof(IDiffItemReplaced<>).MakeGenericType(property.Type)),
						Members.DiffItems.ReplacedNewValue(property.Type)
					)
				),
				Expression.Throw(
					Expression.New(
						typeof(Exception) // TODO: Better exception
					)
				)
			);

			if (algorithm != null)
			{
				applicator = Expression.IfThenElse(
					Expression.TypeIs(item, typeof(IDiffItemChanged)),
					Expression.Assign(
						Expression.Property(ret, property.ReflectionPropertyInfo),
						Expression.Call(
							Expression.Constant(algorithm),
							Members.ApplyPatchAlgorithm.Apply(property.Type),
							Expression.Property(orig, property.ReflectionPropertyInfo),
							Expression.Property(
								Expression.Convert(item, typeof(IDiffItemChanged<>).MakeGenericType(property.Type)),
								Members.DiffItems.ChangedDiff(property.Type)
							)
						)
					),
					applicator
				);
			}

			return
				Expression.Block(
					new[] { item },
					Expression.Assign(
						item,
						Expression.Convert(
							Expression.Property(enumerator, Members.Enumerable.Current(typeof(IDiffItem))),
							typeof(IDiffClassItem)
						)
					),
					Expression.IfThenElse(
						Expression.AndAlso(
							Expression.Not(Expression.TypeIs(item, typeof(IDiffItemUnchanged))),
							Expression.AndAlso(
								someLeft,
								Expression.Equal(
									Expression.Property(
										Expression.Property(
											item,
											Members.DiffItems.ClassProperty()
										),
										Members.FastProperty.UniqueID()
									),
									Expression.Constant(property.UniqueID)
								)
							)
						),
						Expression.Block(
							applicator,
							this.MoveEnumerator(enumerator, someLeft)
						),
						Expression.Assign(
							Expression.Property(ret, property.ReflectionPropertyInfo),
							Expression.Property(orig, property.ReflectionPropertyInfo)
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
