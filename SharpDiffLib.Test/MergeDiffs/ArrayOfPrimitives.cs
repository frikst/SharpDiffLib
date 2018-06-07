using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestFixture]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.MergeOrderedCollectionDiffsRules();
			}
		}

		[Test]
		public void EmptyDiffs()
		{
			var left = DiffResultFactory.Ordered<int>()
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void OnlyLeftAdded()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(5, 5)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void OnlyRightAdded()
		{
			var left = DiffResultFactory.Ordered<int>()
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Added(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedOnDifferentIndexes()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Added(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesRemovedOnDifferentIndexes()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Removed(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.Removed(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedOne()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Added(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Conflicted(
					c => c.Added(3, 3),
					c => c.Added(3, 4)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedSame()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedTwo()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Added(3, 5)
				.Added(3, 6)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Conflicted(
					c => c
						.Added(3, 3)
						.Added(3, 4),
					c => c
						.Added(3, 5)
						.Added(3, 6)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesRemovedOne()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesRemovedTwo()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void AddedAndReplaced()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.Replaced(3, 3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void AddedAndReplacedPlusNonConflicted()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.Added(4, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Added(3, 3)
				.Added(3, 4)
				.Replaced(3, 3, 4)
				.Added(4, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void RemovedAndReplacedPlusNonConflicted()
		{
			var left = DiffResultFactory.Ordered<int>()
				.Removed(3, 3)
				.Removed(4, 4)
				.Added(5, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>()
				.Conflicted(
					c => c.Removed(3, 3),
					c => c.Replaced(3, 3, 4)
				)
				.Removed(4, 4)
				.Added(5, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
