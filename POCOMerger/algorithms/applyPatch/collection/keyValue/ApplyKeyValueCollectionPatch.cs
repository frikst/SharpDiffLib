using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.applyPatch.collection.keyValue
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
				this.aApplyItemDiff = this.aMergerImplementation.Partial.GetApplyPatchAlgorithm<TItemType>();
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
				if (item is IDiffItemAdded<TItemType>)
					ret[item.Key] = (((IDiffItemAdded<TItemType>)item).NewValue);
				else if (item is IDiffItemChanged)
					ret[item.Key] = this.aApplyItemDiff.Apply(
						ret[item.Key],
						((IDiffItemChanged<TItemType>)item).ValueDiff
					);
				else if (item is IDiffItemRemoved<TItemType>)
					ret.Remove(item.Key);
				else if (item is IDiffItemReplaced<TItemType>)
					ret[item.Key] = (((IDiffItemReplaced<TItemType>)item).NewValue);
				else
					throw new InvalidOperationException();
			}

			return this.aConvertor(ret);
		}
	}
}
