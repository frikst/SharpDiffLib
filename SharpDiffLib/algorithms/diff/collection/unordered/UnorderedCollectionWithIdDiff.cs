using System;
using System.Collections.Generic;
using System.Linq;
using SharpDiffLib.algorithms.diff.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.implementation;
using SharpDiffLib.fastReflection;
using SharpDiffLib.implementation;
using SharpDiffLib.@internal;

namespace SharpDiffLib.algorithms.diff.collection.unordered
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
				this.aItemDiff = this.aMergerImplementation.Partial.Algorithms.GetDiffAlgorithm<TItemType>();
				this.aItemComparer = EqualityComparer<TItemType>.Default;
			}

			return this.ComputeInternal((IEnumerable<TItemType>) @base, (IEnumerable<TItemType>) changed);
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

		private IDiff<TType> ComputeInternal(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			Dictionary<TIdType, TItemType> changedSet = changed.ToDictionary(this.aIdAccessor);

			List<IDiffItem> ret = new List<IDiffItem>(20); // 20 seems to be a good value :)

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
						IDiff<TItemType> itemDiff = this.aItemDiff.Compute(baseItem, changedItem);

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
	}
}
