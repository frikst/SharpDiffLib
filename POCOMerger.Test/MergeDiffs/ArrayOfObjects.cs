using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.Test._Entities.SimpleWithId;
using POCOMerger.algorithms.mergeDiffs;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diffResult;

namespace POCOMerger.Test.MergeDiffs
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Ordered<Sample>.Create()
				.Added(5, new Sample { Id = 1, Value = "Hello" })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

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
			Assert.IsTrue(hadConflicts);
		}
	}
}
