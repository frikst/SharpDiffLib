using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.mergeDiffs;
using POCOMerger.definition;
using POCOMerger.diffResult;

namespace POCOMerger.Test.MergeDiffs
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
		}

		[TestMethod]
		public void OnlyLeftAdded()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
		}

		[TestMethod]
		public void OnlyRightAdded()
		{
			var left = DiffResultFactory.Ordered<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Added(3, 3)
				.Added(5, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Ordered<int>.Create()
				.Removed(3, 3)
				.Removed(4, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
		}
	}
}
