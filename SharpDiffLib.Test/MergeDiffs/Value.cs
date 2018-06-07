using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.BaseWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestFixture]
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

		[Test]
		public void MergeEmptyDiffs()
		{
			var left = DiffResultFactory.Value<int>()
				.MakeDiff();
			var right = DiffResultFactory.Value<int>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedLeft()
		{
			var left = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedRight()
		{
			var left = DiffResultFactory.Value<int>()
				.MakeDiff();
			var right = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedBoth()
		{
			var left = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedConflict()
		{
			var left = DiffResultFactory.Value<int>()
				.Replaced(3, 5)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<int>()
				.Conflicted(
					c => c.Replaced(3, 5),
					c => c.Replaced(3, 4)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void MergeObject()
		{
			var left = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleBase>()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleBase>()
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleBase>()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeObjectConflict()
		{
			var left = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleBase>()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleBase>()
					.Replaced(x => x.ValueBase, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleBase>()
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

		[Test]
		public void MergeObjectConflictDescendants()
		{
			var left = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleDescendant1>()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleDescendant1>()
					.Replaced(x => x.ValueBase, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Value<SampleBase>()
				.Changed(DiffResultFactory.Class<SampleDescendant1>()
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
