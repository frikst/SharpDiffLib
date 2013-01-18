﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.diff.collection
{
	internal class UnorderedCollectionWithIdDiff<TType, TIdType, TItemType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<TItemType, TIdType> aIdAccessor;
		private IDiffAlgorithm<TItemType> aItemDiff;
		private readonly Property aIdProperty;
		private EqualityComparer<TItemType> aItemComparer;

		public UnorderedCollectionWithIdDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIdProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aIdAccessor == null)
			{
				this.aIdAccessor = IdHelpers.CreateIdAccessor<TItemType, TIdType>(this.aIdProperty);
				this.aItemDiff = this.aMergerImplementation.Partial.GetDiffAlgorithm<TItemType>();
				this.aItemComparer = EqualityComparer<TItemType>.Default;
			}

			return this.ComputeInternal((IEnumerable<TItemType>) @base, (IEnumerable<TItemType>) changed);
		}

		#endregion

		private IDiff<TType> ComputeInternal(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			Dictionary<TIdType, TItemType> changedSet = changed.ToDictionary(this.aIdAccessor);

			List<IDiffItem> ret = new List<IDiffItem>(); // 20 seems to be a good value :)

			foreach (TItemType baseItem in @base)
			{
				TIdType id = this.aIdAccessor(baseItem);

				TItemType changedItem;

				if (changedSet.TryGetValue(id, out changedItem))
				{
					changedSet.Remove(id);

					if (this.aItemDiff.IsDirect)
					{
						if (!this.aItemComparer.Equals(baseItem, changedItem))
							ret.Add(new DiffUnorderedCollectionReplaced<TItemType>(baseItem, changedItem));
					}
					else
					{
						IDiff itemDiff = this.aItemDiff.Compute(baseItem, changedItem);

						if (itemDiff.Count > 0)
							ret.Add(new DiffUnorderedCollectionChanged<TIdType, TItemType>(id, itemDiff));
					}
				}
				else
					ret.Add(new DiffUnorderedCollectionRemoved<TItemType>(baseItem));
			}

			foreach (KeyValuePair<TIdType, TItemType> item in changedSet)
				ret.Add(new DiffUnorderedCollectionAdded<TItemType>(item.Value));

			return new Diff<TType>(ret);
		}

		#region Implementation of IDiffAlgorithm

		public bool IsDirect
		{
			get { return false; }
		}

		#endregion
	}
}
