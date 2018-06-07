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

		[Test]
		public void OnlyLeftAdded()
		{
			var left = DiffResultFactory.Ordered<Sample>()
				.Added(5, new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();
			var right = DiffResultFactory.Ordered<Sample>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<Sample>()
				.Added(5, new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothChanged()
		{
			var left = DiffResultFactory.Ordered<Sample>()
				.Changed(5, DiffResultFactory.Class<Sample>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<Sample>()
				.Changed(5, DiffResultFactory.Class<Sample>()
					.Replaced(x => x.Value, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<Sample>()
				.Changed(5, DiffResultFactory.Class<Sample>()
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
		public void ReplacedChanged()
		{
			var left = DiffResultFactory.Ordered<Sample>()
				.Changed(0, DiffResultFactory.Class<Sample>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.Ordered<Sample>()
				.Removed(0, new Sample { Id = 1 })
				.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Ordered<Sample>()
					.Conflicted(
						c => c.Changed(0, DiffResultFactory.Class<Sample>()
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
