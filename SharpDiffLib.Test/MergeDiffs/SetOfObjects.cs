using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
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

		[TestMethod]
		public void OnlyLeftAdded()
		{
			var left = DiffResultFactory.Unordered<Sample>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Unordered<Sample>.Create()
				.Added(new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<Sample>.Create()
				.Added(new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothChanged()
		{
			var left = DiffResultFactory.Unordered<Sample>.Create()
				.Changed(1, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<Sample>.Create()
				.Changed(1, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<Sample>.Create()
				.Changed(1, DiffResultFactory.Class<Sample>.Create()
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
		public void ChangedRemoved()
		{
			var left = DiffResultFactory.Unordered<Sample>.Create()
				.Changed(1, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<Sample>.Create()
				.Removed(new Sample { Id = 1, Value = "a" })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<Sample>.Create()
				.Conflicted(
					c => c.Changed(1, DiffResultFactory.Class<Sample>.Create()
						.Replaced(x => x.Value, "a", "b")
						.MakeDiff()
					),
					c => c.Removed(new Sample { Id = 1, Value = "a" })
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
