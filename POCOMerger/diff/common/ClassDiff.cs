using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.@internal;

namespace POCOMerger.diff.common
{
	public class ClassDiff<TType> : IDiffAlgorithm<TType>
	{
		private Func<TType, TType, List<IDiffItem>> aCompiled;

		public ClassDiff()
		{
			this.aCompiled = null;
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aCompiled == null)
			{
				var compiled = this.Compile();

				this.aCompiled = compiled.Compile();
			}

			return new Diff<TType>(this.aCompiled(@base, changed));
		}

		private Expression<Func<TType, TType, List<IDiffItem>>> Compile()
		{
			List<Expression> body = new List<Expression>();
			ParameterExpression ret = Expression.Parameter(typeof(List<IDiffItem>), "ret");
			ParameterExpression @base = Expression.Parameter(typeof(TType), "base");
			ParameterExpression changed = Expression.Parameter(typeof(TType), "changed");

			body.Add(
				Expression.Assign(
					ret,
					Expression.New(
						Members.List.NewWithCount(typeof(IDiffItem)),
						Expression.Constant(Class<TType>.Properties.Count) // maximum number of different items
					)
				)
			);

			foreach (Property property in Class<TType>.Properties)
			{
				body.Add(
					Expression.IfThen(
						Expression.NotEqual(
							Expression.Property(@base, property.ReflectionPropertyInfo),
							Expression.Property(changed, property.ReflectionPropertyInfo)
						),
						Expression.Call(
							ret,
							Members.List.Add(typeof(IDiffItem)),
							Expression.New(
								Members.DiffItems.NewClassReplaced(property.Type),
								Expression.Constant(property),
								Expression.Property(@base, property.ReflectionPropertyInfo),
								Expression.Property(changed, property.ReflectionPropertyInfo)
							)
						)
					)
				);
			}

			body.Add(ret);

			return Expression.Lambda<Func<TType, TType, List<IDiffItem>>>(
				Expression.Block(new[] { ret }, body),
				@base, changed
				);
		}

		#endregion
	}
}