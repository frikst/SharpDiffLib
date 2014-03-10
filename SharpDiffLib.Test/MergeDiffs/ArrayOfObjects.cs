using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.SimpleWithId;
using SharpDiffLib.algorithms.mergeDiffs;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
	public class ArrayOfObjects
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

				Define<Sample[]>()
					.MergeOrderedCollectionDiffsRules();
			}
		}

		[TestMethod]
		public void OnlyLeftAdded()
		{
			var left = DiffResultFactory.Ordered<Sample>.Create()
				.Added(5, new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();
			var right = DiffResultFactory.Ordered<Sample>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<Sample>.Create()
				.Added(5, new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothChanged()
		{
			var left = DiffResultFactory.Ordered<Sample>.Create()
				.Changed(5, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<Sample>.Create()
				.Changed(5, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<Sample>.Create()
				.Changed(5, DiffResultFactory.Class<Sample>.Create()
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
		public void ReplacedChanged()
		{
			var left = DiffResultFactory.Ordered<Sample>.Create()
				.Changed(0, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<Sample>.Create()
				.Removed(0, new Sample { Id = 1 })
				.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<Sample>.Create()
					.Conflicted(
						c => c.Changed(0, DiffResultFactory.Class<Sample>.Create()
							.Replaced(x => x.Value, "a", "b")
							.MakeDiff()
						),
						c => c.Removed(0, new Sample { Id = 1 })
					)
					.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
