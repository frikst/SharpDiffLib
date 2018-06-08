using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestFixture]
	public class SetOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.MergeClassDiffsRules();

				Define<HashSet<Sample>>()
					.MergeUnorderedCollectionDiffsRules();
			}
		}

		[Test]
		public void OnlyLeftAdded()
		{
			var left = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.MakeDiff();
			var right = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Added(new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Added(new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothChanged()
		{
			var left = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Changed(1, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Changed(1, inner => inner.Class()
					.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Changed(1, inner => inner.Class()
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
		public void ChangedRemoved()
		{
			var left = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Changed(1, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Removed(new Sample { Id = 1, Value = "a" })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Conflicted(
					c => c.Changed(1, inner => inner.Class()
						.Replaced(x => x.Value, "a", "b")
					),
					c => c.Removed(new Sample { Id = 1, Value = "a" })
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
