using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
	public class SimpleClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.MergeClassDiffsRules();
			}
		}

		[TestMethod]
		public void MergeEmptyDiffs()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
			    .MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeNonConflictingDiffs()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeNonConflictingDiffsReverseOrder()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeConflictingDiffs()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "c")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeSameChanges()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeUnchangedReplaced()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeUnchangedUnchanged()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
