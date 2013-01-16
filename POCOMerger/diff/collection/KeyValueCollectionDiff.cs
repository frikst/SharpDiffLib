using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.diff.collection
{
	internal class KeyValueCollectionDiff<TType, TKeyType, TItemType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<TItemType, TItemType, bool> aIsTheSame;
		private IDiffAlgorithm<TItemType> aItemDiff;
		private readonly Property aIdProperty;

		public KeyValueCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIsTheSame = null;

			this.aIdProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aIsTheSame == null)
			{
				this.aIsTheSame = IdHelpers.CompileIsTheSame<TItemType>(this.aIdProperty);
				this.aItemDiff = this.aMergerImplementation.Partial.GetDiffAlgorithm<TItemType>();
			}

			return this.ComputeInternal((IEnumerable<KeyValuePair<TKeyType, TItemType>>)@base, (IEnumerable<KeyValuePair<TKeyType, TItemType>>)changed);
		}

		#endregion

		public IDiff<TType> ComputeInternal(IEnumerable<KeyValuePair<TKeyType, TItemType>> @base, IEnumerable<KeyValuePair<TKeyType, TItemType>> changed)
		{
			List<IDiffItem> ret = new List<IDiffItem>(); // 20 seems to be a good value :)

			IDictionary<TKeyType, TItemType> changedDict = new Dictionary<TKeyType, TItemType>();

			foreach (KeyValuePair<TKeyType, TItemType> keyValuePair in changed)
				changedDict.Add(keyValuePair);

			foreach (KeyValuePair<TKeyType, TItemType> baseKeyValue in @base)
			{
				TItemType changedItem;

				if (changedDict.TryGetValue(baseKeyValue.Key, out changedItem))
				{
					changedDict.Remove(baseKeyValue.Key);

					if (this.aIsTheSame(baseKeyValue.Value, changedItem))
					{
						if (this.aIdProperty != null && this.aItemDiff != null)
						{
							IDiff itemDiff = this.aItemDiff.Compute(baseKeyValue.Value, changedItem);

							if (itemDiff.Count > 0)
								ret.Add(new DiffKeyValueCollectionChanged<TKeyType, TItemType>(baseKeyValue.Key, itemDiff));
						}
					}
					else
						ret.Add(new DiffKeyValueCollectionReplaced<TKeyType, TItemType>(baseKeyValue.Key, baseKeyValue.Value, changedItem));
				}
				else
					ret.Add(new DiffKeyValueCollectionRemoved<TKeyType, TItemType>(baseKeyValue.Key, baseKeyValue.Value));
			}

			foreach (KeyValuePair<TKeyType, TItemType> item in changedDict)
				ret.Add(new DiffKeyValueCollectionAdded<TKeyType, TItemType>(item.Key, item.Value));

			return new Diff<TType>(ret);
		}
	}
}
