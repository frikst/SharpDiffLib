using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
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

		[TestMethod]
		public void EmptyDiffs()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void OnlyLeftAdded()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void OnlyRightAdded()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothSidesAddedOnDifferentIndexes()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothSidesRemovedOnDifferentIndexes()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Removed(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.Removed(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothSidesAddedOne()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Conflicted(
					c => c.Added(3, 3),
					c => c.Added(3, 4)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothSidesAddedSame()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothSidesAddedTwo()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 5)
				.Added(3, 6)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
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

		[TestMethod]
		public void BothSidesRemovedOne()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothSidesRemovedTwo()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void AddedAndReplaced()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.Replaced(3, 3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void AddedAndReplacedPlusNonConflicted()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.Added(4, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(3, 4)
				.Replaced(3, 3, 4)
				.Added(4, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void RemovedAndReplacedPlusNonConflicted()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.Removed(4, 4)
				.Added(5, 4)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
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
