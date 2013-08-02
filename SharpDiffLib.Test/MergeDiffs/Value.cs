using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.BaseWithId;
using SharpDiffLib.Test._Entities.SimpleWithId;
using SharpDiffLib.algorithms.mergeDiffs;
using SharpDiffLib.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
	public class Value
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.MergeValueDiffsRules()
					.MergeClassDiffsRules();
				Define<SampleDescendant1>()
					.MergeClassDiffsRules();
				Define<int>()
					.MergeValueDiffsRules();
			}
		}

		[TestMethod]
		public void MergeEmptyDiffs()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeIntReplacedLeft()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeIntReplacedRight()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeIntReplacedBoth()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeIntReplacedConflict()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 5)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Conflicted(
					c => c.Replaced(3, 5),
					c => c.Replaced(3, 4)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeObject()
		{
			var left = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeObjectConflict()
		{
			var left = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.Replaced(x => x.ValueBase, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.Conflicted(
						c => c.Replaced(x => x.ValueBase, "a", "b"),
						c => c.Replaced(x => x.ValueBase, "a", "c")
					)
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void MergeObjectConflictDescendants()
		{
			var left = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleDescendant1>.Create()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleDescendant1>.Create()
					.Replaced(x => x.ValueBase, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleDescendant1>.Create()
					.Conflicted(
						c => c.Replaced(x => x.ValueBase, "a", "b"),
						c => c.Replaced(x => x.ValueBase, "a", "c")
					)
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
