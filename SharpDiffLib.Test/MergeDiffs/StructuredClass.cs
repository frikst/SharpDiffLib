using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.InnerClassWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestFixture]
	public class StructuredClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleInner>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.MergeClassDiffsRules();

				Define<Sample>()
					.MergeClassDiffsRules();
			}
		}

		[Test]
		public void MergeEmptyDiffs()
		{
			var left = DiffResultFactory.Class<Sample>()
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingDiffsWithInnerChanges()
		{
			var left = DiffResultFactory.Class<Sample>()
				.Changed(x=>x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x=>x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeConflictingDiffsWithInnerChanges()
		{
			var left = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Conflicted(
						c => c.Replaced(x => x.Value, "a", "b"),
						c => c.Replaced(x => x.Value, "a", "c")
					)
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingDiffsWithSameInnerChanges()
		{
			var left = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingRightItemLast()
		{
			var left = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value2, "a", "b")
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
