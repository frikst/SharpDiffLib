using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.applyPatch.common.@class
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
			ParameterExpression notChanged = Expression.Parameter(typeof(HashSet<Property>), "notChanged");
			ParameterExpression itemNotChanged = Expression.Parameter(typeof(Property), "item");
			ParameterExpression ret = Expression.Parameter(typeof(TType), "ret");
			ParameterExpression orig = Expression.Parameter(typeof(TType), "orig");
			ParameterExpression diff = Expression.Parameter(typeof(IDiff), "diff");
			ParameterExpression item = Expression.Parameter(typeof(IDiffClassItem), "item");

			return Expression.Lambda<Func<TType, IDiff, TType>>(
				Expression.Block(
					new[] { ret, notChanged },
					Expression.Assign(
						ret,
						Expression.New(typeof(TType))
					),
					Expression.Assign(
						notChanged,
						Expression.New(
							Members.HashSet.NewHashSetFromEnumerable(typeof(Property)),
							Expression.Constant(Class<TType>.Properties.ToArray())
						)
					),
					ExpressionExtensions.ForEach(
						item,
						diff,
						Expression.Block(
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
							),
							Expression.Call(
								notChanged,
								Members.HashSet.Remove(typeof(Property)),
								Expression.Property(item, Members.DiffItems.ClassProperty())
							)
						)
					),
					ExpressionExtensions.ForEach(
						itemNotChanged,
						notChanged,
						Expression.Switch(
							typeof(void),
							Expression.Property(itemNotChanged, Members.FastProperty.UniqueID()),
							null,
							null,
							Class<TType>
									.Properties
									.Select(x => this.CompileCaseDefaultValue(x, ret, orig, itemNotChanged))
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
							Expression.Convert(
								Expression.Property(
									Expression.Convert(item, typeof(IDiffItemChanged)),
									Members.DiffItems.ChangedDiff()
								),
								typeof(IDiff<>).MakeGenericType(property.Type)
							)
						)
					),
					caseBody
				);
			}

			return Expression.SwitchCase(caseBody, Expression.Constant(property.UniqueID));
		}

		private SwitchCase CompileCaseDefaultValue(Property property, ParameterExpression ret, ParameterExpression orig, ParameterExpression itemNotChanged)
		{
			return Expression.SwitchCase(
				Expression.Assign(
					Expression.Property(ret, property.ReflectionPropertyInfo),
					Expression.Property(orig, property.ReflectionPropertyInfo)
				),
				Expression.Constant(property.UniqueID)
			);
		}
	}
}
