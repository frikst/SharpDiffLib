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

		[Test]
		public void EmptyDiffs()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void LeftAdded()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothAdded()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothAddedDifferent()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 1 })
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.Added("a", new Sample { Id = 2 })
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.Conflicted(
					c => c.Added("a", new Sample { Id = 1 }),
					c => c.Added("a", new Sample { Id = 2 })
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[Test]
		public void BothChanged()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class())
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothChangedSame()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[Test]
		public void BothChangedWithConflicts()
		{
			var left = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
				)
				.MakeDiff();
			var right = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.KeyValue<string, Sample>()
				.Changed("a", inner => inner.Class()
					.Conflicted(
						c => c.Replaced(x => x.Value, "a", "b"),
						c => c.Replaced(x => x.Value, "a", "c")
					)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}
	}
}
