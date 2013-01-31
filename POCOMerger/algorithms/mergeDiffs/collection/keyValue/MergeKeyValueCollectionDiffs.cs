using System.Collections.Generic;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.diffResult.type;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.mergeDiffs.collection.keyValue
{
	internal class MergeKeyValueCollectionDiffs<TType, TKeyType, TItemType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly EqualityComparer<TKeyType> aComparer;
		private IMergeDiffsAlgorithm<TItemType> aMergeItemsDiffs;

		public MergeKeyValueCollectionDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aComparer = EqualityComparer<TKeyType>.Default;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			if (this.aMergeItemsDiffs == null)
				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.GetMergeDiffsAlgorithm<TItemType>();


			hadConflicts = false;

			Dictionary<TKeyType, IDiffKeyValueCollectionItem<TKeyType>> rightIndex = new Dictionary<TKeyType, IDiffKeyValueCollectionItem<TKeyType>>(right.Count);

			foreach (IDiffItem item in right)
				rightIndex[((IDiffKeyValueCollectionItem<TKeyType>) item).Key] = (IDiffKeyValueCollectionItem<TKeyType>) item;

			List<IDiffItem> ret = new List<IDiffItem>(left.Count + right.Count);

			foreach (IDiffKeyValueCollectionItem<TKeyType> leftItem in left)
			{
				IDiffKeyValueCollectionItem<TKeyType> rightItem;

				if (rightIndex.TryGetValue(leftItem.Key, out rightItem))
				{
					rightIndex.Remove(leftItem.Key);

					if (this.ProcessConflict(leftItem.Key, leftItem, rightItem, ret))
						hadConflicts = true;
				}
				else
					ret.Add(leftItem);
			}

			ret.AddRange(rightIndex.Values);

			return new Diff<TType>(ret);
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, out bool hadConflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, out hadConflicts);
		}

		#endregion

		private bool ProcessConflict(TKeyType key, IDiffKeyValueCollectionItem<TKeyType> leftItem, IDiffKeyValueCollectionItem<TKeyType> rightItem, List<IDiffItem> ret)
		{
			if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
				return false;
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem);
				return false;
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				IDiff<TItemType> diffLeft = ((IDiffItemChanged<TItemType>)leftItem).ValueDiff;
				IDiff<TItemType> diffRight = ((IDiffItemChanged<TItemType>)rightItem).ValueDiff;

				bool hadConflicts;
				IDiff<TItemType> result = this.aMergeItemsDiffs.MergeDiffs(diffLeft, diffRight, out hadConflicts);

				ret.Add(new DiffKeyValueCollectionItemChanged<TKeyType, TItemType>(key, result));

				return hadConflicts;
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
				return false;
			}

			ret.Add(new DiffAnyConflicted(leftItem, rightItem));
			return true;
		}
	}
}
