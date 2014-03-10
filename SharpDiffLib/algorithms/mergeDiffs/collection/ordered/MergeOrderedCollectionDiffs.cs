using System;
using System.Collections.Generic;
using System.Linq;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.implementation;
using SharpDiffLib.diffResult.type;
using SharpDiffLib.implementation;

namespace SharpDiffLib.algorithms.mergeDiffs.collection.ordered
{
	internal class MergeOrderedCollectionDiffs<TType, TItemType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		private IMergeDiffsAlgorithm<TItemType> aMergeItemsDiffs;

		public MergeOrderedCollectionDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aMergeItemsDiffs = null;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			if (this.aMergeItemsDiffs == null)
				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm<TItemType>();

			List<IDiffItem> ret = new List<IDiffItem>();

			foreach (var diffItem in new MergeJoin(new Chunker(left.Cast<IDiffOrderedCollectionItem>()), new Chunker(right.Cast<IDiffOrderedCollectionItem>())))
			{
				if (diffItem.Item1 == null)
					ret.AddRange(diffItem.Item2);
				else if (diffItem.Item2 == null)
					ret.AddRange(diffItem.Item1);
				else
					this.ProcessConflicts(ret, diffItem.Item1, diffItem.Item2, conflicts);
			}

			return new Diff<TType>(ret);
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private void ProcessConflicts(List<IDiffItem> ret, List<IDiffOrderedCollectionItem> left, List<IDiffOrderedCollectionItem> right, IConflictContainer conflicts)
		{
			List<IDiffOrderedCollectionItem> addedLeft;
			List<IDiffOrderedCollectionItem> addedRight;

			IDiffOrderedCollectionItem otherLeft;
			IDiffOrderedCollectionItem otherRight;

			this.SepareAdditions(left, out addedLeft, out otherLeft);
			this.SepareAdditions(right, out addedRight, out otherRight);

			if (addedRight != null && addedLeft != null)
			{
				DiffAnyConflicted conflict = new DiffAnyConflicted(addedLeft, addedRight);
				ret.Add(conflict);
				conflicts.RegisterConflict(conflict);
			}
			else if (addedLeft != null)
			{
				ret.AddRange(addedLeft);
			}
			else if (addedRight != null)
			{
				ret.AddRange(addedRight);
			}

			if (otherLeft != null && otherRight != null)
				ret.Add(this.ProcessConflict(otherLeft, otherRight, conflicts));
			else if (otherLeft != null)
				ret.Add(otherLeft);
			else if (otherRight != null)
				ret.Add(otherRight);
		}

		private void SepareAdditions(List<IDiffOrderedCollectionItem> input, out List<IDiffOrderedCollectionItem> added, out IDiffOrderedCollectionItem other)
		{
			if (input.Last() is IDiffItemAdded)
			{
				other = null;
				added = input;
			}
			else if (input.Count == 1)
			{
				other = input.Last();
				added = null;
			}
			else
			{
				other = input.Last();
				input.RemoveAt(input.Count - 1);
				added = input;
			}

			if (added != null && !added.All(x => x is IDiffItemAdded))
				throw new Exception("Error in input diff");
		}

		private IDiffItem ProcessConflict(IDiffOrderedCollectionItem left, IDiffOrderedCollectionItem right, IConflictContainer conflicts)
		{
			if (left is IDiffItemChanged && right is IDiffItemChanged)
			{
				IDiff<TItemType> diff = this.aMergeItemsDiffs.MergeDiffs(
					((IDiffItemChanged<TItemType>)left).ValueDiff,
					((IDiffItemChanged<TItemType>)right).ValueDiff,
					conflicts
				);

				return new DiffOrderedCollectionChanged<TItemType>(left.ItemIndex, diff);
			}
			else if (left.IsSame(right))
			{
				return left;
			}

			DiffAnyConflicted conflict = new DiffAnyConflicted(left, right);
			conflicts.RegisterConflict(conflict);
			return conflict;
		}
	}
}
