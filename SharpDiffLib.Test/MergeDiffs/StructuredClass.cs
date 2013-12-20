using SharpDiffLib.Test._Entities.InnerClassWithId;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.algorithms.mergeDiffs;
using SharpDiffLib.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
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
		public void MergeNonConflictingDiffsWithInnerChanges()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Changed(x=>x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x=>x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeConflictingDiffsWithInnerChanges()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
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

		[TestMethod]
		public void MergeNonConflictingDiffsWithSameInnerChanges()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeNonConflictingRightItemLast()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value2, "a", "b")
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
