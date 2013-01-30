using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

			List<IDiffItem> ret = new List<IDiffItem>();

			int indexDeltaLeft = 0;
			int indexDeltaRight = 0;
			int indexDeltaRet = 0;

			IEnumerator<IDiffItem> rightEnumerator = right.GetEnumerator();
			IEnumerator<IDiffItem> leftEnumerator = left.GetEnumerator();

			IDiffOrderedCollectionItem rightItem = this.NextItem(rightEnumerator, ref indexDeltaRight);
			IDiffOrderedCollectionItem leftItem = this.NextItem(leftEnumerator, ref indexDeltaLeft);

			while (leftItem != null)
			{
				int leftIndex = leftItem.ItemIndex + indexDeltaLeft;

				while (rightItem != null && rightItem.ItemIndex + indexDeltaRight < leftIndex)
				{
					this.AddItemToRet(rightItem, indexDeltaRight, ret, ref indexDeltaRet);

					rightItem = this.NextItem(rightEnumerator, ref indexDeltaRight);
				}

				if (rightItem != null && rightItem.ItemIndex + indexDeltaRight == leftIndex)
				{
					List<IDiffItem> leftBlock = new List<IDiffItem>();

					while (leftItem != null && leftItem.ItemIndex == leftIndex)
					{
						leftBlock.Add(leftItem.CreateWithDelta(indexDeltaLeft + indexDeltaRet));
						leftItem = this.NextItem(leftEnumerator, ref indexDeltaLeft);
					}

					List<IDiffItem> rightBlock = new List<IDiffItem>();

					while (rightItem != null && rightItem.ItemIndex == leftIndex)
					{
						rightBlock.Add(rightItem.CreateWithDelta(indexDeltaRight + indexDeltaRet));
						rightItem = this.NextItem(rightEnumerator, ref indexDeltaRight);
					}

					if (leftBlock.Count == 1 && rightBlock.Count == 1)
					{
						if (this.ProcessConflict((IDiffOrderedCollectionItem) leftBlock[0], (IDiffOrderedCollectionItem) rightBlock[0], ret, ref indexDeltaRet))
							hadConflicts = true;
					}
					else
					{
						if (this.ProcessConflict(leftBlock, rightBlock, ret, ref indexDeltaRet))
							hadConflicts = true;
					}
				}
				else
				{
					this.AddItemToRet(leftItem, indexDeltaLeft, ret, ref indexDeltaRet);
					leftItem = this.NextItem(leftEnumerator, ref indexDeltaLeft);
				}
			}

			while (rightItem != null)
			{
				this.AddItemToRet(rightItem, indexDeltaRight, ret, ref indexDeltaRet);
				rightItem = this.NextItem(rightEnumerator, ref indexDeltaRight);
			}

			return new Diff<TType>(ret);
		}

		#endregion

		private bool ProcessConflict(List<IDiffItem> leftItem, List<IDiffItem> rightItem, List<IDiffItem> ret, ref int indexDeltaRet)
		{
			Lazy<bool> allLeftAdded = new Lazy<bool>(() => leftItem.All(x => x is IDiffItemAdded));
			Lazy<bool> allLeftRemoved = new Lazy<bool>(() => leftItem.All(x => x is IDiffItemRemoved));
			Lazy<bool> allRightAdded = new Lazy<bool>(() => rightItem.All(x => x is IDiffItemAdded));
			Lazy<bool> allRightRemoved = new Lazy<bool>(() => rightItem.All(x => x is IDiffItemRemoved));

			if (allLeftAdded.Value && leftItem.SequenceEqual(rightItem, (a, b) => a.IsSame(b)))
			{
				foreach (IDiffOrderedCollectionItem item in leftItem)
					this.AddItemToRet(item, 0, ret, ref indexDeltaRet);
				return false;
			}
			else if (allLeftRemoved.Value && leftItem.SequenceEqual(rightItem, (a, b) => a.IsSame(b)))
			{
				foreach (IDiffOrderedCollectionItem item in leftItem)
					this.AddItemToRet(item, 0, ret, ref indexDeltaRet);
				return false;
			}
			else if (leftItem.Count == 1 && (leftItem[0] is IDiffItemReplaced || leftItem[0] is IDiffItemChanged) && (allRightAdded.Value || allRightRemoved.Value))
			{
				foreach (IDiffOrderedCollectionItem item in rightItem)
					this.AddItemToRet(item, 0, ret, ref indexDeltaRet);
				this.AddItemToRet((IDiffOrderedCollectionItem)leftItem[0], 0, ret, ref indexDeltaRet);
				return false;
			}
			else if (rightItem.Count == 1 && (rightItem[0] is IDiffItemReplaced || rightItem[0] is IDiffItemChanged) && (allLeftAdded.Value || allLeftRemoved.Value))
			{
				foreach (IDiffOrderedCollectionItem item in leftItem)
					this.AddItemToRet(item, 0, ret, ref indexDeltaRet);
				this.AddItemToRet((IDiffOrderedCollectionItem)rightItem[0], 0, ret, ref indexDeltaRet);
				return false;
			}
			else
			{
				ret.Add(new DiffAnyConflicted(leftItem, rightItem));
				return true;
			}
		}

		private bool ProcessConflict(IDiffOrderedCollectionItem leftItem, IDiffOrderedCollectionItem rightItem, List<IDiffItem> ret, ref int indexDeltaRet)
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

		private IDiffOrderedCollectionItem NextItem(IEnumerator<IDiffItem> rightEnumerator, ref int itemIndex)
		{
			IDiffOrderedCollectionItem ret = (IDiffOrderedCollectionItem) (rightEnumerator.MoveNext() ? rightEnumerator.Current : null);

			if (ret is IDiffItemRemoved)
				itemIndex++;

			return ret;
		}

		private void AddItemToRet(IDiffOrderedCollectionItem item, int indexDelta, List<IDiffItem> ret, ref int indexDeltaRet)
		{
			if (item is IDiffItemRemoved)
				indexDeltaRet--;
			ret.Add(item.CreateWithDelta(indexDelta + indexDeltaRet));
		}
	}
}
