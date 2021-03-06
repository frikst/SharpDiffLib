﻿using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Implementation;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.Algorithms.Diff.Collection.KeyValue
{
	internal class KeyValueCollectionDiff<TType, TKeyType, TItemType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<TItemType, TItemType, bool> aIsTheSame;
		private IDiffAlgorithm<TItemType> aItemDiff;
		private readonly Property aIdProperty;
		private IEqualityComparer<TItemType> aItemComparer;

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
				this.aItemDiff = this.aMergerImplementation.Partial.Algorithms.GetDiffAlgorithm<TItemType>();
				this.aItemComparer = EqualityComparer<TItemType>.Default;
			}

			return this.ComputeInternal((IEnumerable<KeyValuePair<TKeyType, TItemType>>)@base, (IEnumerable<KeyValuePair<TKeyType, TItemType>>)changed);
		}

		#endregion

		#region Implementation of IDiffAlgorithm

		public bool IsDirect
			=> false;

		IDiff IDiffAlgorithm.Compute(object @base, object changed)
		{
			if (!(@base is TType && changed is TType))
				throw new InvalidOperationException("base and changed has to be of type " + typeof(TType).Name);

			return this.Compute((TType)@base, (TType)changed);
		}

		#endregion

		private IDiff<TType> ComputeInternal(IEnumerable<KeyValuePair<TKeyType, TItemType>> @base, IEnumerable<KeyValuePair<TKeyType, TItemType>> changed)
		{
			List<IDiffItem> ret = new List<IDiffItem>(20); // 20 seems to be a good value :)

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
						if (this.aIdProperty != null)
						{
							if (this.aItemDiff.IsDirect)
							{
								if (!this.aItemComparer.Equals(baseKeyValue.Value, changedItem))
									ret.Add(new DiffKeyValueCollectionItemReplaced<TKeyType, TItemType>(baseKeyValue.Key, baseKeyValue.Value, changedItem));
							}
							else
							{
								IDiff<TItemType> itemDiff = this.aItemDiff.Compute(baseKeyValue.Value, changedItem);

								if (itemDiff.HasChanges)
									ret.Add(new DiffKeyValueCollectionItemChanged<TKeyType, TItemType>(baseKeyValue.Key, itemDiff));
							}
						}
					}
					else
					{
						if (this.aIdProperty != null || this.aItemDiff.IsDirect)
							ret.Add(new DiffKeyValueCollectionItemReplaced<TKeyType, TItemType>(baseKeyValue.Key, baseKeyValue.Value, changedItem));
						else
						{
							IDiff<TItemType> itemDiff = this.aItemDiff.Compute(baseKeyValue.Value, changedItem);

							if (itemDiff.HasChanges)
								ret.Add(new DiffKeyValueCollectionItemChanged<TKeyType, TItemType>(baseKeyValue.Key, itemDiff));
						}
					}
				}
				else
					ret.Add(new DiffKeyValueCollectionItemRemoved<TKeyType, TItemType>(baseKeyValue.Key, baseKeyValue.Value));
			}

			foreach (KeyValuePair<TKeyType, TItemType> item in changedDict)
				ret.Add(new DiffKeyValueCollectionItemAdded<TKeyType, TItemType>(item.Key, item.Value));

			return new Diff<TType>(ret);
		}
	}
}
