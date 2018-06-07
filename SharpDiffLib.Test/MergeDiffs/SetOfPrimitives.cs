using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestFixture]
	public class SetOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<int>>()
					.MergeUnorderedCollectionDiffsRules();
			}
		}

		[Test]
		public void EmptyDiffs()
		{
			var left = DiffResultFactory.Unordered<int>()
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyLeftAdded()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyRightAdded()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.Added(4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Added(3)
				.Added(4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyBothAddedTheSame()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyRemovedReplaced()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Removed(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.Replaced(3, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Conflicted(
					c => c.Removed(3),
					c => c.Replaced(3, 5)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyBothRemovedTheSame()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Removed(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.Removed(3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Removed(3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyReplacedReplaced()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.Replaced(3, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Conflicted(
					c => c.Replaced(3, 4),
					c => c.Replaced(3, 5)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void EmptyReplacedReplacedBothSame()
		{
			var left = DiffResultFactory.Unordered<int>()
				.Replaced(3, 5)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>()
				.Replaced(3, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>()
				.Replaced(3, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
