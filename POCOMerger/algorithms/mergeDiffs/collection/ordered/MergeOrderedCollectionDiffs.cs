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

		public MergeOrderedCollectionDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
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
					ret.Add(this.ProcessConflict(leftItem, rightItem, indexDeltaLeft + indexDeltaRet));
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

		private IDiffItem ProcessConflict(IDiffOrderedCollectionItem leftitem, IDiffOrderedCollectionItem rightItem, int leftDelta)
		{
			throw new NotImplementedException();
		}

		private IDiffOrderedCollectionItem NextItem(IEnumerator<IDiffItem> rightEnumerator)
		{
			return (IDiffOrderedCollectionItem)(rightEnumerator.MoveNext() ? rightEnumerator.Current : null);
		}
	}
}
