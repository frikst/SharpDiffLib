using System;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.applyPatch.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.applyPatch.common.@class
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
			ParameterExpression ret = Expression.Parameter(typeof(TType), "ret");
			ParameterExpression orig = Expression.Parameter(typeof(TType), "orig");
			ParameterExpression diff = Expression.Parameter(typeof(IDiff), "diff");
			ParameterExpression item = Expression.Parameter(typeof(IDiffClassItem), "item");

			return Expression.Lambda<Func<TType, IDiff, TType>>(
				Expression.Block(
					new [] { ret },
					Expression.Assign(
						ret,
						Expression.New(typeof(TType))
					),
					ExpressionExtensions.ForEach(
						item,
						diff,
						Expression.Switch(
							Expression.Property(
								Expression.Property(item, Members.DiffItems.ClassProperty()),
								Members.FastProperty.UniqueID()
							),
							Expression.Throw(
								Expression.New(
									typeof(Exception) // TODO: Better exception
								)
							),
							null,
							Class<TType>
								.Properties
								.Select(x => this.CompileCase(x, ret, orig, item))
								.Where(x => x != null)
						)
					),
					ret
				),
				orig, diff
			);
		}

		private SwitchCase CompileCase(Property property, ParameterExpression ret, ParameterExpression orig, ParameterExpression item)
		{
			IApplyPatchAlgorithm algorithm = this.aMergerImplementation.Partial.GetApplyPatchAlgorithm(property.Type);

			Expression caseBody = Expression.IfThenElse(
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
				caseBody = Expression.IfThenElse(
					Expression.TypeIs(item, typeof(IDiffItemChanged)),
					Expression.Assign(
						Expression.Property(ret, property.ReflectionPropertyInfo),
						Expression.Call(
							Expression.Constant(algorithm),
							Members.ApplyPatchAlgorithm.Apply(property.Type),
							Expression.Property(orig, property.ReflectionPropertyInfo),
							Expression.Property(
								Expression.Convert(item, typeof(IDiffItemChanged)),
								Members.DiffItems.ChangedDiff()
							)
						)
					),
					caseBody
				);
			}

			return Expression.SwitchCase(caseBody, Expression.Constant(property.UniqueID));
		}
	}
}
