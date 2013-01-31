using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.Test._Entities.BaseWithId;
using POCOMerger.Test._Entities.SimpleWithId;
using POCOMerger.algorithms.mergeDiffs;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diffResult;

namespace POCOMerger.Test.MergeDiffs
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
					.MergeValueDiffsRules();
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
		}

		[TestMethod]
		public void MergeIntReplacedLeft()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
		}

		[TestMethod]
		public void MergeIntReplacedRight()
		{
			var left = DiffResultFactory.Value<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Value<int>.Create()
				.Conflicted(
					c => c.Replaced(3, 5),
					c => c.Replaced(3, 4)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Value<SampleBase>.Create()
				.Changed(DiffResultFactory.Class<SampleBase>.Create()
					.Replaced(x => x.ValueBase, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

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
			Assert.IsTrue(hadConflicts);
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

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

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
			Assert.IsTrue(hadConflicts);
		}
	}
}
