using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.algorithms.mergeDiffs.@base;
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

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			if (this.aMergeItemsDiffs == null)
				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.GetMergeDiffsAlgorithm<TItemType>();
			hadConflicts = false;

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
					{
						if (this.ProcessConflict(leftIterator.Items[0], rightIterator.Items[0], ret))
							hadConflicts = true;
					}
					else
					{
						if (this.ProcessConflict(leftIterator.Items, rightIterator.Items, ret))
							hadConflicts = true;
					}

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

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, out bool hadConflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, out hadConflicts);
		}

		#endregion

		private bool ProcessConflict(List<IDiffOrderedCollectionItem> leftItem, List<IDiffOrderedCollectionItem> rightItem, AutoindexedResult ret)
		{
			Lazy<bool> allLeftAdded = new Lazy<bool>(() => leftItem.All(x => x is IDiffItemAdded));
			Lazy<bool> allLeftRemoved = new Lazy<bool>(() => leftItem.All(x => x is IDiffItemRemoved));
			Lazy<bool> allRightAdded = new Lazy<bool>(() => rightItem.All(x => x is IDiffItemAdded));
			Lazy<bool> allRightRemoved = new Lazy<bool>(() => rightItem.All(x => x is IDiffItemRemoved));

			if (allLeftAdded.Value && leftItem.SequenceEqual(rightItem, (a, b) => a.IsSame(b)))
			{
				ret.AddRange(leftItem);
				return false;
			}
			else if (leftItem.Count == rightItem.Count && allLeftRemoved.Value && allRightRemoved.Value)
			{
				ret.AddRange(leftItem);
				return false;
			}
			else if (leftItem.Count == 1 && (leftItem[0] is IDiffItemReplaced || leftItem[0] is IDiffItemChanged) && allRightAdded.Value)
			{
				ret.AddRange(rightItem);
				ret.AddRange(leftItem);
				return false;
			}
			else if (rightItem.Count == 1 && (rightItem[0] is IDiffItemReplaced || rightItem[0] is IDiffItemChanged) && allLeftAdded.Value)
			{
				ret.AddRange(leftItem);
				ret.AddRange(rightItem);
				return false;
			}
			else
			{
				ret.Add(new DiffAnyConflicted(leftItem, rightItem));
				return true;
			}
		}

		private bool ProcessConflict(IDiffOrderedCollectionItem leftItem, IDiffOrderedCollectionItem rightItem, AutoindexedResult ret)
		{
			if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded)
			{
				if (aEqualityComparer.Equals(((IDiffItemAdded<TItemType>)leftItem).NewValue, ((IDiffItemAdded<TItemType>)rightItem).NewValue))
				{
					ret.Add(leftItem);
					return false;
				}
				else
				{
					ret.Add(new DiffAnyConflicted(leftItem, rightItem));
					return true;
				}
			}
			else if (leftItem is IDiffItemAdded || rightItem is IDiffItemAdded)
			{
				if (leftItem is IDiffItemRemoved || !(rightItem is IDiffItemRemoved))
				{
					ret.Add(leftItem);
					ret.Add(rightItem);
					return false;
				}
				else
				{
					ret.Add(rightItem);
					ret.Add(leftItem);
					return false;
				}
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem);
				return false;
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				bool hadConflicts;

				IDiff<TItemType> diff = this.aMergeItemsDiffs.MergeDiffs(
					((IDiffItemChanged<TItemType>)leftItem).ValueDiff,
					((IDiffItemChanged<TItemType>)rightItem).ValueDiff,
					out hadConflicts
				);

				ret.Add(new DiffOrderedCollectionChanged<TItemType>(leftItem.ItemIndex, diff));

				return hadConflicts;
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced)
			{
				if (aEqualityComparer.Equals(((IDiffItemReplaced<TItemType>) leftItem).NewValue, ((IDiffItemReplaced<TItemType>) rightItem).NewValue))
				{
					ret.Add(leftItem);
					return false;
				}
				else
				{
					ret.Add(new DiffAnyConflicted(leftItem, rightItem));
					return true;
				}
			}
			else
			{
				ret.Add(new DiffAnyConflicted(leftItem, rightItem));
				return true;
			}
		}
	}
}
