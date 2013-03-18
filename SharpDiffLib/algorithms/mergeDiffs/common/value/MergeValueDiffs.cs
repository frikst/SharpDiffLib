using System;
using System.Collections.Generic;
using System.Linq;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.implementation;
using SharpDiffLib.implementation;

namespace SharpDiffLib.algorithms.mergeDiffs.common.value
{
	internal class MergeValueDiffs<TType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		public MergeValueDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			List<IDiffItem> ret = new List<IDiffItem>(1);

			if (left.Count == 0 && right.Count == 1)
				ret.AddRange(right);
			else if (left.Count == 1 && right.Count == 0)
				ret.AddRange(left);
			else if (left.Count == 1 && right.Count == 1)
				this.ProcessConflict(left.First(), right.First(), ret, conflicts);
			else if (left.Count > 0 && right.Count > 0)
				throw new Exception();

			return new Diff<TType>(ret);
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private void ProcessConflict(IDiffItem leftItem, IDiffItem rightItem, List<IDiffItem> ret, IConflictContainer conflicts)
		{
			if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				Type itemType = leftItem.ItemType;

				IMergeDiffsAlgorithm mergeItemsDiffs = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm(itemType);

				IDiff diffLeft = ((IDiffItemChanged) leftItem).ValueDiff;
				IDiff diffRight = ((IDiffItemChanged) rightItem).ValueDiff;

				IDiff result = mergeItemsDiffs.MergeDiffs(diffLeft, diffRight, conflicts);

				ret.Add(new DiffValueChanged<TType>(itemType, (IDiff<TType>) result));
			}
			else if (leftItem is IDiffItemUnchanged)
			{
				ret.Add(rightItem);
			}
			else if (rightItem is IDiffItemUnchanged)
			{
				ret.Add(leftItem);
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
