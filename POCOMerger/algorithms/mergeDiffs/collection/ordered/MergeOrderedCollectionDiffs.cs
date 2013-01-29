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

			IDiffOrderedCollectionItem rightItem = this.NextItem(rightEnumerator);

			if (rightItem is IDiffItemRemoved)
				indexDeltaRight++;

			foreach (IDiffOrderedCollectionItem leftItem in left)
			{
				if (leftItem is IDiffItemRemoved)
					indexDeltaLeft++;

				while (rightItem != null && rightItem.ItemIndex + indexDeltaRight < leftItem.ItemIndex + indexDeltaLeft)
				{
					if (rightItem is IDiffItemRemoved)
						indexDeltaRet--;
					ret.Add(rightItem.CreateWithDelta(indexDeltaRight + indexDeltaRet));

					rightItem = this.NextItem(rightEnumerator);
					if (rightItem is IDiffItemRemoved)
						indexDeltaRight++;
				}

				if (rightItem != null && rightItem.ItemIndex + indexDeltaRight == leftItem.ItemIndex + indexDeltaLeft)
				{
					if (this.ProcessConflict(leftItem, rightItem, indexDeltaLeft + indexDeltaRet, indexDeltaRight + indexDeltaRet, ret))
						hadConflicts = true;

					rightItem = this.NextItem(rightEnumerator);
					if (rightItem is IDiffItemRemoved)
						indexDeltaRight++;
				}
				else
				{
					if (leftItem is IDiffItemRemoved)
						indexDeltaRet--;
					ret.Add(leftItem.CreateWithDelta(indexDeltaLeft + indexDeltaRet));
				}
			}

			if (rightItem is IDiffItemRemoved)
				indexDeltaRet--;

			while (rightItem != null)
			{
				ret.Add(rightItem.CreateWithDelta(indexDeltaRight + indexDeltaRet));
				rightItem = this.NextItem(rightEnumerator);
			}

			return new Diff<TType>(ret);
		}

		#endregion

		private bool ProcessConflict(IDiffOrderedCollectionItem leftItem, IDiffOrderedCollectionItem rightItem, int leftDelta, int rightDelta, List<IDiffItem> ret)
		{
			if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded)
			{
				ret.Add(new DiffAnyConflicted(leftItem.CreateWithDelta(leftDelta), rightItem.CreateWithDelta(rightDelta)));
				return true;
			}
			else if (leftItem is IDiffItemAdded || rightItem is IDiffItemAdded)
			{
				if (leftItem is IDiffItemRemoved || !(rightItem is IDiffItemRemoved))
				{
					ret.Add(leftItem.CreateWithDelta(leftDelta));
					ret.Add(rightItem.CreateWithDelta(leftDelta));
					return false;
				}
				else
				{
					ret.Add(rightItem.CreateWithDelta(leftDelta));
					ret.Add(leftItem.CreateWithDelta(leftDelta));
					return false;
				}
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem.CreateWithDelta(leftDelta));
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

				ret.Add(new DiffOrderedCollectionChanged<TItemType>(leftItem.ItemIndex + leftDelta, diff));

				return hadConflicts;
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced)
			{
				if (aEqualityComparer.Equals(((IDiffItemReplaced<TItemType>) leftItem).NewValue, ((IDiffItemReplaced<TItemType>) rightItem).NewValue))
				{
					ret.Add(leftItem.CreateWithDelta(leftDelta));
					return false;
				}
				else
				{
					ret.Add(new DiffAnyConflicted(leftItem.CreateWithDelta(leftDelta), rightItem.CreateWithDelta(rightDelta)));
					return true;
				}
			}
			else
			{
				ret.Add(new DiffAnyConflicted(leftItem.CreateWithDelta(leftDelta), rightItem.CreateWithDelta(rightDelta)));
				return true;
			}
		}

		private IDiffOrderedCollectionItem NextItem(IEnumerator<IDiffItem> rightEnumerator)
		{
			return (IDiffOrderedCollectionItem)(rightEnumerator.MoveNext() ? rightEnumerator.Current : null);
		}
	}
}
