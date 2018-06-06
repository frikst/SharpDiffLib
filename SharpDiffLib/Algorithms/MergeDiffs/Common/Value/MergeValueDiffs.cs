using System;
using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Common.Value
{
	internal class MergeValueDiffs<TType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly IAlgorithmRules aRules;

		public MergeValueDiffs(MergerImplementation mergerImplementation, IAlgorithmRules rules)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aRules = rules;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			List<IDiffItem> ret = new List<IDiffItem>(1);

			if (!left.HasChanges && right.HasChanges)
				ret.AddRange(right);
			else if (left.HasChanges && !right.HasChanges)
				ret.AddRange(left);
			else if (left.HasChanges && right.HasChanges)
				this.ProcessConflict((IDiffValue) left.First(), (IDiffValue) right.First(), ret, conflicts);

			return new Diff<TType>(ret);
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private void ProcessConflict(IDiffValue leftItem, IDiffValue rightItem, List<IDiffItem> ret, IConflictContainer conflicts)
		{
			if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemChanged leftChanged && rightItem is IDiffItemChanged rightChanged)
			{
				Type itemType = leftItem.ValueType;

				IMergeDiffsAlgorithm mergeItemsDiffs = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm(itemType, this.aRules);

				IDiff diffLeft = leftChanged.ValueDiff;
				IDiff diffRight = rightChanged.ValueDiff;

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
