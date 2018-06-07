using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.IntArrayProperty;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class CollectionAsItem
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.OrderedCollectionDiffRules();

				Define<Sample>()
					.ClassDiffRules();

				Define<Dictionary<string, int[]>>()
					.KeyValueCollectionDiffRules();
			}
		}

		[Test]
		public void ClassOneAdded()
		{
			var @base = new Sample { Value = new[] { 1, 2 } };
			var changed = new Sample { Value = new[] { 1, 2, 3 } };

			var expected = DiffResultFactory.Class<Sample>()
				.Changed(x => x.Value, DiffResultFactory.Ordered<int>()
					.Added(2, 3)
					.MakeDiff()
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void ClassUnchanged()
		{
			var @base = new Sample { Value = new[] { 1, 2, 3 } };
			var changed = new Sample { Value = new[] { 1, 2, 3 } };

			var expected = DiffResultFactory.Class<Sample>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void DictionaryOneAdded()
		{
			var @base = new Dictionary<string, int[]> { { "a", new[] { 1, 2 } } };
			var changed = new Dictionary<string, int[]> { { "a", new[] { 1, 2, 3 } } };

			var expected = DiffResultFactory.KeyValue<string, int[]>()
				.Changed("a", DiffResultFactory.Ordered<int>()
					.Added(2, 3)
					.MakeDiff()
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void DictionaryUnchanged()
		{
			var @base = new Dictionary<string, int[]> { { "a", new[] { 1, 2, 3 } } };
			var changed = new Dictionary<string, int[]> { { "a", new[] { 1, 2, 3 } } };

			var expected = DiffResultFactory.KeyValue<string, int[]>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
