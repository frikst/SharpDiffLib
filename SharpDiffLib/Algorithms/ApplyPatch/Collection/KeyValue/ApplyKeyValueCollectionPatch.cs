using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.KeyValue
{
	internal class ApplyKeyValueCollectionPatch<TType, TKeyType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<Dictionary<TKeyType, TItemType>, TType> aConvertor;
		private IApplyPatchAlgorithm<TItemType> aApplyItemDiff;

		public ApplyKeyValueCollectionPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aConvertor = null;
			this.aApplyItemDiff = null;
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
			if (this.aConvertor == null)
			{
				this.aConvertor = this.CompileConvertor();
				this.aApplyItemDiff = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm<TItemType>();
			}

			return this.ApplyInternal((IEnumerable<KeyValuePair<TKeyType, TItemType>>)source, patch);
		}

		#endregion

		private Func<Dictionary<TKeyType, TItemType>, TType> CompileConvertor()
		{
			if (typeof(TType).IsGenericType
							&& (typeof(TType).GetGenericTypeDefinition() == typeof(Dictionary<,>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(IDictionary<,>)))
				return x => (TType)(object)x;

			ParameterExpression from = Expression.Parameter(typeof(Dictionary<TKeyType, TItemType>), "from");

			Expression<Func<Dictionary<TKeyType, TItemType>, TType>> convertor =
				Expression.Lambda<Func<Dictionary<TKeyType, TItemType>, TType>>(
					Expression.New(
						typeof(TType).GetConstructor(new[] { typeof(IDictionary<TKeyType, TItemType>) }),
						Expression.Convert(from, typeof(IDictionary<TKeyType, TItemType>))
					),
					from
				);

			return convertor.Compile();
		}

		private TType ApplyInternal(IEnumerable<KeyValuePair<TKeyType, TItemType>> source, IDiff<TType> patch)
		{
			Dictionary<TKeyType, TItemType> ret = new Dictionary<TKeyType, TItemType>();

			foreach (KeyValuePair<TKeyType, TItemType> item in source)
				ret[item.Key] = item.Value;

			foreach (IDiffKeyValueCollectionItem<TKeyType> item in patch)
			{
				switch (item)
				{
					case IDiffItemAdded<TItemType> itemAdded:
						ret[item.Key] = itemAdded.NewValue;
						break;
					case IDiffItemChanged<TItemType> itemChanged:
						ret[item.Key] = this.aApplyItemDiff.Apply(ret[item.Key], itemChanged.ValueDiff);
						break;
					case IDiffItemRemoved<TItemType> _:
						ret.Remove(item.Key);
						break;
					case IDiffItemReplaced<TItemType> itemReplaced:
						ret[item.Key] = itemReplaced.NewValue;
						break;
					default:
						throw new InvalidOperationException();
				}
			}

			return this.aConvertor(ret);
		}
	}
}
