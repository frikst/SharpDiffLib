﻿using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.KeyValue
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

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			if (this.aMergeItemsDiffs == null)
				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm<TItemType>();

			Dictionary<TKeyType, IDiffKeyValueCollectionItem<TKeyType>> rightIndex = new Dictionary<TKeyType, IDiffKeyValueCollectionItem<TKeyType>>(right.Count);

			foreach (IDiffItem item in right)
				rightIndex[((IDiffKeyValueCollectionItem<TKeyType>) item).Key] = (IDiffKeyValueCollectionItem<TKeyType>) item;

			List<IDiffItem> ret = new List<IDiffItem>(left.Count + right.Count);

			foreach (var leftItem in left.Cast<IDiffKeyValueCollectionItem<TKeyType>>())
			{
				IDiffKeyValueCollectionItem<TKeyType> rightItem;

				if (rightIndex.TryGetValue(leftItem.Key, out rightItem))
				{
					rightIndex.Remove(leftItem.Key);

					this.ProcessConflict(leftItem.Key, leftItem, rightItem, ret, conflicts);
				}
				else
					ret.Add(leftItem);
			}

			ret.AddRange(rightIndex.Values);

			return new Diff<TType>(ret);
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private void ProcessConflict(TKeyType key, IDiffKeyValueCollectionItem<TKeyType> leftItem, IDiffKeyValueCollectionItem<TKeyType> rightItem, List<IDiffItem> ret, IConflictContainer conflicts)
		{
			if (leftItem is IDiffItemUnchanged)
			{
				ret.Add(rightItem);
			}
			else if (rightItem is IDiffItemUnchanged)
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				IDiff<TItemType> diffLeft = ((IDiffItemChanged<TItemType>)leftItem).ValueDiff;
				IDiff<TItemType> diffRight = ((IDiffItemChanged<TItemType>)rightItem).ValueDiff;

				IDiff<TItemType> result = this.aMergeItemsDiffs.MergeDiffs(diffLeft, diffRight, conflicts);

				ret.Add(new DiffKeyValueCollectionItemChanged<TKeyType, TItemType>(key, result));
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
				ret.Add(leftItem);
			else
			{
				DiffAnyConflicted conflict = new DiffAnyConflicted(leftItem, rightItem);
				ret.Add(conflict);
				conflicts.RegisterConflict(conflict);
			}
		}
	}
}
