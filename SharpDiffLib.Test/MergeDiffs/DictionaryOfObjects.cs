using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
	public class DictionaryOfObjects
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

				Define<Dictionary<string, Sample>>()
					.MergeKeyValueCollectionDiffsRules();
			}
		}

		[TestMethod]
		public void EmptyDiffs()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void LeftAdded()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothAdded()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothAddedDifferent()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Added("a", new Sample { Id = 2 })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Conflicted(
					c => c.Added("a", new Sample { Id = 1 }),
					c => c.Added("a", new Sample { Id = 2 })
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothChanged()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothChangedSame()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void BothChangedWithConflicts()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "c")
					.MakeDiff()
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("a", DiffResultFactory.Class<Sample>.Create()
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
	}
}
