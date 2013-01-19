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

namespace POCOMerger.diff.common.@class
{
	internal class ClassDiff<TType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<TType, TType, List<IDiffItem>> aCompiled;

		public ClassDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
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

		#endregion

		#region Implementation of IDiffAlgorithm

		public bool IsDirect
		{
			get { return false; }
		}

		IDiff IDiffAlgorithm.Compute(object @base, object changed)
		{
			if (!(@base is TType && changed is TType))
				throw new InvalidOperationException("base and changed has to be of type " + typeof(TType).Name);

			return this.Compute((TType)@base, (TType)changed);
		}

		#endregion

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
				Property idProperty = GeneralRulesHelper.GetIdProperty(this.aMergerImplementation, property.Type);

				if (idProperty != null)
					body.Add(this.MightBeChanged(ret, property, idProperty, @base, changed));
				else
					body.Add(this.MightBeReplaced(ret, property, @base, changed));
			}

			body.Add(ret);

			return Expression.Lambda<Func<TType, TType, List<IDiffItem>>>(
				Expression.Block(new[] { ret }, body),
				@base, changed
			);
		}

		private ConditionalExpression MightBeReplaced(ParameterExpression ret, Property property, ParameterExpression @base, ParameterExpression changed)
		{
			MemberExpression baseProperty = Expression.Property(@base, property.ReflectionPropertyInfo);
			MemberExpression changedProperty = Expression.Property(changed, property.ReflectionPropertyInfo);

			return Expression.IfThen(
				Expression.NotEqual(
					baseProperty,
					changedProperty
				),
				this.NewDiffReplaced(ret, property, @base, changed)
			);
		}

		private ConditionalExpression MightBeChanged(ParameterExpression ret, Property property, Property id, ParameterExpression @base, ParameterExpression changed)
		{
			MemberExpression baseProperty = Expression.Property(@base, property.ReflectionPropertyInfo);
			MemberExpression changedProperty = Expression.Property(changed, property.ReflectionPropertyInfo);

			MemberExpression baseId = Expression.Property(baseProperty, id.ReflectionPropertyInfo);
			MemberExpression changedId = Expression.Property(changedProperty, id.ReflectionPropertyInfo);

			return Expression.IfThenElse(
				Expression.Or(
					Expression.ReferenceEqual(
						baseProperty,
						Expression.Constant(null)
					),
					Expression.ReferenceEqual(
						changedProperty,
						Expression.Constant(null)
					)
				),
				Expression.IfThen(
					Expression.ReferenceNotEqual(
						baseProperty,
						changedProperty
					),
					this.NewDiffReplaced(ret, property, @base, changed)
				),
				Expression.IfThenElse(
					Expression.NotEqual(
						baseId,
						changedId
					),
					this.NewDiffReplaced(ret, property, @base, changed),
					this.NewDiffChanged(ret, property, @base, changed)
				)
			);
		}

		private MethodCallExpression NewDiffReplaced(ParameterExpression ret, Property property, ParameterExpression @base, ParameterExpression changed)
		{
			MemberExpression baseProperty = Expression.Property(@base, property.ReflectionPropertyInfo);
			MemberExpression changedProperty = Expression.Property(changed, property.ReflectionPropertyInfo);

			return Expression.Call(
				ret,
				Members.List.Add(typeof(IDiffItem)),
				Expression.New(
					Members.DiffItems.NewClassReplaced(property.Type),
					Expression.Constant(property),
					baseProperty,
					changedProperty
				)
			);
		}

		private Expression NewDiffChanged(ParameterExpression ret, Property property, ParameterExpression @base, ParameterExpression changed)
		{
			IDiffAlgorithm diff = this.aMergerImplementation.Partial.GetDiffAlgorithm(property.Type);

			if (diff.IsDirect)
				return this.NewDiffReplaced(ret, property, @base, changed);

			MemberExpression baseProperty = Expression.Property(@base, property.ReflectionPropertyInfo);
			MemberExpression changedProperty = Expression.Property(changed, property.ReflectionPropertyInfo);

			ParameterExpression tmp = Expression.Parameter(typeof(IDiff), "tmp");

			return Expression.Block(
				new[] { tmp },
				Expression.Assign(
					tmp,
					Expression.Call(
						Expression.Constant(diff),
						Members.DiffAlgorithm.Compute(property.Type),
						baseProperty,
						changedProperty
					)
				),
				Expression.IfThen(
					Expression.NotEqual(
						Expression.Property(tmp, Members.Diff.Count()),
						Expression.Constant(0)
					),
					Expression.Call(
						ret,
						Members.List.Add(typeof(IDiffItem)),
						Expression.New(
							Members.DiffItems.NewClassChanged(property.Type),
							Expression.Constant(property),
							tmp
						)
					)
				)
			);
		}
	}
}