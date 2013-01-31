using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.mergeDiffs.common.value
{
	internal class MergeValueDiffs<TType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		public MergeValueDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			List<IDiffItem> ret = new List<IDiffItem>(1);
			hadConflicts = false;

			if (left.Count == 0 && right.Count == 1)
				ret.AddRange(right);
			else if (left.Count == 1 && right.Count == 0)
				ret.AddRange(left);
			else if (left.Count == 1 && right.Count == 1)
			{
				if (this.ProcessConflict(left.First(), right.First(), ret))
					hadConflicts = true;
			}
			else if (left.Count > 0 && right.Count > 0)
				throw new Exception();

			return new Diff<TType>(ret);
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, out bool hadConflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, out hadConflicts);
		}

		#endregion

		private bool ProcessConflict(IDiffItem leftItem, IDiffItem rightItem, List<IDiffItem> ret)
		{
			if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
				return false;
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				Type itemType = leftItem.ItemType;

				IMergeDiffsAlgorithm mergeItemsDiffs = this.aMergerImplementation.Partial.GetMergeDiffsAlgorithm(itemType);

				IDiff diffLeft = ((IDiffItemChanged)leftItem).ValueDiff;
				IDiff diffRight = ((IDiffItemChanged)rightItem).ValueDiff;

				bool hadConflicts;
				IDiff result = mergeItemsDiffs.MergeDiffs(diffLeft, diffRight, out hadConflicts);

				ret.Add(new DiffValueChanged<TType>(itemType, (IDiff<TType>) result));

				return hadConflicts;
			}

			ret.Add(new DiffAnyConflicted(leftItem, rightItem));
			return true;
		}
	}
}
