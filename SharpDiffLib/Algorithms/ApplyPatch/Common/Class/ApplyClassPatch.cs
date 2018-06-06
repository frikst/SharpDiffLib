using System;
using System.Linq;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Implementation;
using KST.SharpDiffLib.Internal;
using KST.SharpDiffLib.Internal.Members;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Common.Class
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
			ParameterExpression item = Expression.Parameter(typeof(IDiffClassItem), "item");

			return Expression.Lambda<Func<TType, IDiff, TType>>(
				Expression.Block(
					new[] { ret },
					Expression.Assign(
						ret,
						Expression.New(typeof(TType))
					),
					Expression.Block(Class<TType>.Properties.Select(property =>
						Expression.Assign(
							Expression.Property(ret, property.ReflectionPropertyInfo),
							Expression.Property(orig, property.ReflectionPropertyInfo)
						)
					)),
					ExpressionExtensions.ForEach(
						item,
						diff,
						Expression.IfThen(
							Expression.Not(Expression.TypeIs(item, typeof(IDiffItemUnchanged))),
							Expression.Block(
								Expression.Switch(
									Expression.Property(
										Expression.Property(
											item,
											DiffItemsMembers.ClassProperty()
										),
										FastPropertyMembers.UniqueID()
									),
									Expression.Throw(
										Expression.New(typeof(Exception))
									),
									Class<TType>.Properties.Select(property => this.EvaluateProperty(ret, orig, property, item)).ToArray()
								)
							)
						)
					),
					ret
				),
				orig, diff
			);
		}

		private SwitchCase EvaluateProperty(ParameterExpression ret, ParameterExpression orig, Property property, ParameterExpression item)
		{
			IApplyPatchAlgorithm algorithm = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm(property.Type);

			Expression applicator = Expression.IfThenElse(
				Expression.TypeIs(item, typeof(IDiffItemReplaced)),
				Expression.Assign(
					Expression.Property(ret, property.ReflectionPropertyInfo),
					Expression.Property(
						Expression.Convert(item, typeof(IDiffItemReplaced<>).MakeGenericType(property.Type)),
						DiffItemsMembers.ReplacedNewValue(property.Type)
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
							ApplyPatchAlgorithmMembers.Apply(property.Type),
							Expression.Property(orig, property.ReflectionPropertyInfo),
							Expression.Property(
								Expression.Convert(item, typeof(IDiffItemChanged<>).MakeGenericType(property.Type)),
								DiffItemsMembers.ChangedDiff(property.Type)
							)
						)
					),
					applicator
				);
			}

			return Expression.SwitchCase(applicator, Expression.Constant(property.UniqueID));
		}
	}
}
