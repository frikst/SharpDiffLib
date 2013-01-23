using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.diff.common.value
{
	internal class ValueDiff<TType> : IDiffAlgorithm<TType>
	{
		private Func<TType, TType, List<IDiffItem>> aCompiledDiff;
		private readonly MergerImplementation aMergerImplementation;
		private Property aIDProperty;

		public ValueDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIDProperty = GeneralRulesHelper<TType>.GetIdProperty(mergerImplementation);
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aCompiledDiff == null)
			{
				this.aCompiledDiff = this.CompileDiff();
			}

			return new Diff<TType>(this.aCompiledDiff(@base, changed));
		}

		#endregion

		#region Implementation of IDiffAlgorithm

		public bool IsDirect
		{
			get { return true; }
		}

		IDiff IDiffAlgorithm.Compute(object @base, object changed)
		{
			if (!(@base is TType && changed is TType))
				throw new InvalidOperationException("base and changed has to be of type " + typeof(TType).Name);

			return this.Compute((TType)@base, (TType)changed);
		}

		#endregion

		private Func<TType, TType, List<IDiffItem>> CompileDiff()
		{
			ParameterExpression ret = Expression.Parameter(typeof(List<IDiffItem>), "ret");

			ParameterExpression @base = Expression.Parameter(typeof(TType), "base");
			ParameterExpression changed = Expression.Parameter(typeof(TType), "changed");

			Expression differ;

			if (this.aIDProperty == null)
			{
				differ = Expression.IfThen(
					ExpressionExtensions.NotEqual(@base, changed),
					Expression.Call(
						ret,
						Members.List.Add(typeof(IDiffItem)),
						Expression.New(
							Members.DiffItems.NewValueReplaced(typeof(TType)),
							@base,
							changed
						)
					)
				);
			}
			else
			{
				differ = Expression.IfThen(
					ExpressionExtensions.NotEqual(
						Expression.Property(@base, this.aIDProperty.ReflectionPropertyInfo),
						Expression.Property(changed, this.aIDProperty.ReflectionPropertyInfo)
					),
					Expression.Call(
						ret,
						Members.List.Add(typeof(IDiffItem)),
						Expression.New(
							Members.DiffItems.NewValueReplaced(typeof(TType)),
							@base,
							changed
						)
					)
				);
			}

			if (!typeof(TType).IsValueType)
			{
				differ = Expression.IfThenElse(
					Expression.Or(
						Expression.ReferenceEqual(@base, Expression.Constant(null)),
						Expression.ReferenceEqual(changed, Expression.Constant(null))
					),
					Expression.IfThen(
						Expression.ReferenceEqual(@base, changed),
						Expression.New(
							Members.DiffItems.NewValueReplaced(typeof(TType)),
							@base,
							changed
						)
					),
					differ
				);
			}

			Expression<Func<TType, TType, List<IDiffItem>>> comparer = Expression.Lambda<Func<TType, TType, List<IDiffItem>>>(
				Expression.Block(
					new[] { ret },
					Expression.Assign(
						ret,
						Expression.New(
							Members.List.NewWithCount(typeof(IDiffItem)),
							Expression.Constant(1) // maximum number of changes
						)
					),
					differ,
					ret
				),
				@base,
				changed
			);

			return comparer.Compile();
		}
	}
}
