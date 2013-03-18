using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.@base;
using POCOMerger.conflictManagement;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.diffResult.type;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.mergeDiffs.collection.ordered
{
	internal class MergeOrderedCollectionDiffs<TType, TItemType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly IEqualityComparer<TItemType> aEqualityComparer;
		private IMergeDiffsAlgorithm<TItemType> aMergeItemsDiffs;

		public MergeOrderedCollectionDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aEqualityComparer = EqualityComparer<TItemType>.Default;
			this.aMergeItemsDiffs = null;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			if (this.aMergeItemsDiffs == null)
				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm<TItemType>();

			AutoindexedResult ret = new AutoindexedResult(left.Count + right.Count);

			BlockIterator leftIterator = new BlockIterator(left);
			BlockIterator rightIterator = new BlockIterator(right);

			while (!leftIterator.IsEnd)
			{
				while (!rightIterator.IsEnd && rightIterator.BlockIndex < leftIterator.BlockIndex)
				{
					ret.AddRange(rightIterator.Items);
					rightIterator.Next();
				}

				if (!rightIterator.IsEnd && leftIterator.BlockIndex == rightIterator.BlockIndex)
				{
					if (leftIterator.Items.Count == 1 && rightIterator.Items.Count == 1)
						this.ProcessConflict(leftIterator.Items[0], rightIterator.Items[0], ret, conflicts);
					else
						this.ProcessConflict(leftIterator.Items, rightIterator.Items, ret, conflicts);

					rightIterator.Next();
				}
				else
					ret.AddRange(leftIterator.Items);

				leftIterator.Next();
			}

			while (!rightIterator.IsEnd)
			{
				ret.AddRange(rightIterator.Items);
				rightIterator.Next();
			}

			return new Diff<TType>(ret.ToList());
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private void ProcessConflict(List<IDiffOrderedCollectionItem> leftItem, List<IDiffOrderedCollectionItem> rightItem, AutoindexedResult ret, IConflictContainer conflicts)
		{
			if (leftItem.Count == 1 && leftItem[0] is IDiffItemUnchanged)
			{
				ret.AddRange(rightItem);
				return;
			}
			else if (rightItem.Count == 1 && rightItem[0] is IDiffItemUnchanged)
			{
				ret.AddRange(leftItem);
				return;
			}
			Lazy<bool> allLeftAdded = new Lazy<bool>(() => leftItem.All(x => x is IDiffItemAdded));
			Lazy<bool> allLeftRemoved = new Lazy<bool>(() => leftItem.All(x => x is IDiffItemRemoved));
			Lazy<bool> allRightAdded = new Lazy<bool>(() => rightItem.All(x => x is IDiffItemAdded));
			Lazy<bool> allRightRemoved = new Lazy<bool>(() => rightItem.All(x => x is IDiffItemRemoved));

			if (allLeftAdded.Value && leftItem.SequenceEqual(rightItem, (a, b) => a.IsSame(b)))
			{
				ret.AddRange(leftItem);
			}
			else if (leftItem.Count == rightItem.Count && allLeftRemoved.Value && allRightRemoved.Value)
			{
				ret.AddRange(leftItem);
			}
			else if (leftItem.Count == 1 && (leftItem[0] is IDiffItemReplaced || leftItem[0] is IDiffItemChanged) && allRightAdded.Value)
			{
				ret.AddRange(rightItem);
				ret.AddRange(leftItem);
			}
			else if (rightItem.Count == 1 && (rightItem[0] is IDiffItemReplaced || rightItem[0] is IDiffItemChanged) && allLeftAdded.Value)
			{
				ret.AddRange(leftItem);
				ret.AddRange(rightItem);
			}
			else
			{
				DiffAnyConflicted conflict = new DiffAnyConflicted(leftItem, rightItem);
				ret.Add(conflict);
				conflicts.RegisterConflict(conflict);
			}
		}

		private void ProcessConflict(IDiffOrderedCollectionItem leftItem, IDiffOrderedCollectionItem rightItem, AutoindexedResult ret, IConflictContainer conflicts)
		{
			if (leftItem is IDiffItemUnchanged)
			{
				ret.Add(rightItem);
			}
			else if (rightItem is IDiffItemUnchanged)
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded)
			{
				if (aEqualityComparer.Equals(((IDiffItemAdded<TItemType>) leftItem).NewValue, ((IDiffItemAdded<TItemType>) rightItem).NewValue))
					ret.Add(leftItem);
				else
				{
					DiffAnyConflicted conflict = new DiffAnyConflicted(leftItem, rightItem);
					ret.Add(conflict);
					conflicts.RegisterConflict(conflict);
				}
			}
			else if (leftItem is IDiffItemAdded || rightItem is IDiffItemAdded)
			{
				if (leftItem is IDiffItemRemoved || !(rightItem is IDiffItemRemoved))
				{
					ret.Add(leftItem);
					ret.Add(rightItem);
				}
				else
				{
					ret.Add(rightItem);
					ret.Add(leftItem);
				}
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				IDiff<TItemType> diff = this.aMergeItemsDiffs.MergeDiffs(
					((IDiffItemChanged<TItemType>) leftItem).ValueDiff,
					((IDiffItemChanged<TItemType>) rightItem).ValueDiff,
					conflicts
					);

				ret.Add(new DiffOrderedCollectionChanged<TItemType>(leftItem.ItemIndex, diff));
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced)
			{
				if (aEqualityComparer.Equals(((IDiffItemReplaced<TItemType>) leftItem).NewValue, ((IDiffItemReplaced<TItemType>) rightItem).NewValue))
					ret.Add(leftItem);
				else
				{
					DiffAnyConflicted conflict = new DiffAnyConflicted(leftItem, rightItem);
					ret.Add(conflict);
					conflicts.RegisterConflict(conflict);
				}
			}
			else
			{
				DiffAnyConflicted conflict = new DiffAnyConflicted(leftItem, rightItem);
				ret.Add(conflict);
				conflicts.RegisterConflict(conflict);
			}
		}
	}
}
