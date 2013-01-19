using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.diff.common.value
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

		private Func<TType, TType, List<IDiffItem>> CompileDiff()
		{
			ParameterExpression ret = Expression.Parameter(typeof(List<IDiffItem>), "ret");

			ParameterExpression @base = Expression.Parameter(typeof(TType), "base");
			ParameterExpression changed = Expression.Parameter(typeof(TType), "changed");

			Expression differ;

			if (this.aIDProperty == null)
			{
				differ = Expression.IfThen(
					Expression.NotEqual(@base, changed),
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
					Expression.NotEqual(
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

		#region Implementation of IDiffAlgorithm

		public bool IsDirect
		{
			get { return true; }
		}

		#endregion
	}
}
