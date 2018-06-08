using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestFixture]
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

		[Test]
		public void MergeEmptyDiffs()
		{
			var left = DiffFactory.Create<Sample>().Class()
			    .MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingDiffs()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingDiffsReverseOrder()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeConflictingDiffs()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "c")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void MergeSameChanges()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeUnchangedReplaced()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeUnchangedUnchanged()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Unchanged(x => x.Value, "a")
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
