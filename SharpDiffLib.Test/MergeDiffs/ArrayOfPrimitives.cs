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
			var left = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void OnlyLeftAdded()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(5, 5)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void OnlyRightAdded()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Added(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedOnDifferentIndexes()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Added(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesRemovedOnDifferentIndexes()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Removed(5, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.Removed(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedOne()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
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
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesAddedTwo()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 5)
				.Added(3, 6)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
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
			var left = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothSidesRemovedTwo()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.Removed(4, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void AddedAndReplaced()
		{
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(3, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
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
			var left = DiffFactory.Create<int[]>().Ordered()
				.Added(3, 3)
				.Added(3, 4)
				.Added(4, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
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
			var left = DiffFactory.Create<int[]>().Ordered()
				.Removed(3, 3)
				.Removed(4, 4)
				.Added(5, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int[]>().Ordered()
				.Replaced(3, 3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int[]>().Ordered()
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
