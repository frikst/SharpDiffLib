using System;
using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Ordered
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

			foreach (var (leftItems, rightItems) in new MergeJoin(new Chunker(left.Cast<IDiffOrderedCollectionItem>()), new Chunker(right.Cast<IDiffOrderedCollectionItem>())))
			{
				if (leftItems == null)
					ret.AddRange(rightItems);
				else if (rightItems == null)
					ret.AddRange(leftItems);
				else
					this.ProcessConflicts(ret, leftItems, rightItems, conflicts);
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
				if (addedLeft.Count == addedRight.Count && addedLeft.Zip(addedRight, (a, b) => a.IsSame(b)).All(x => x))
				{
					ret.AddRange(addedLeft);
				}
				else
				{
					DiffAnyConflicted conflict = new DiffAnyConflicted(addedLeft, addedRight);
					ret.Add(conflict);
					conflicts.RegisterConflict(conflict);
				}
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
