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
			var left = DiffFactory.Create<int>().Value()
				.MakeDiff();
			var right = DiffFactory.Create<int>().Value()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int>().Value()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedLeft()
		{
			var left = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int>().Value()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedRight()
		{
			var left = DiffFactory.Create<int>().Value()
				.MakeDiff();
			var right = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedBoth()
		{
			var left = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeIntReplacedConflict()
		{
			var left = DiffFactory.Create<int>().Value()
				.Replaced(3, 5)
				.MakeDiff();
			var right = DiffFactory.Create<int>().Value()
				.Replaced(3, 4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<int>().Value()
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
			var left = DiffFactory.Create<SampleBase>().Value()
				.Changed(inner => inner.Class()
					.Replaced(x => x.ValueBase, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<SampleBase>().Value()
				.Changed(inner => inner.Class())
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<SampleBase>().Value()
				.Changed(inner => inner.Class()
					.Replaced(x => x.ValueBase, "a", "b")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeObjectConflict()
		{
			var left = DiffFactory.Create<SampleBase>().Value()
				.Changed(inner => inner.Class()
					.Replaced(x => x.ValueBase, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<SampleBase>().Value()
				.Changed(inner => inner.Class()
					.Replaced(x => x.ValueBase, "a", "c")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<SampleBase>().Value()
				.Changed(inner => inner.Class()
					.Conflicted(
						c => c.Replaced(x => x.ValueBase, "a", "b"),
						c => c.Replaced(x => x.ValueBase, "a", "c")
					)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void MergeObjectConflictDescendants()
		{
			var left = DiffFactory.Create<SampleBase>().Value()
				.ChangedType<SampleDescendant1>(inner => inner.Class()
					.Replaced(x => x.ValueBase, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<SampleBase>().Value()
				.ChangedType<SampleDescendant1>(inner => inner.Class()
					.Replaced(x => x.ValueBase, "a", "c")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<SampleBase>().Value()
				.ChangedType<SampleDescendant1>(inner => inner.Class()
					.Conflicted(
						c => c.Replaced(x => x.ValueBase, "a", "b"),
						c => c.Replaced(x => x.ValueBase, "a", "c")
					)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
