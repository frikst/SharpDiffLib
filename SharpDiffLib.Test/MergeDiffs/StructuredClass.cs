﻿using KST.SharpDiffLib.Algorithms.MergeDiffs;
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
		public void MergeNonConflictingDiffsWithInnerChanges()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Changed(x=>x.ValueInner, inner => inner.Class()
					.Replaced(x=>x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class())
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeConflictingDiffsWithInnerChanges()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Conflicted(
						c => c.Replaced(x => x.Value, "a", "b"),
						c => c.Replaced(x => x.Value, "a", "c")
					)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingDiffsWithSameInnerChanges()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void MergeNonConflictingRightItemLast()
		{
			var left = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.MakeDiff();
			var right = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value2, "a", "b")
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "a", "b")
				.Replaced(x => x.Value2, "a", "b")
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
