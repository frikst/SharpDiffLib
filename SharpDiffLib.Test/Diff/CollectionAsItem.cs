using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
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
			const string diff =
				"=Value:\r\n" +
				"\t+2:3";
			var @base = new Sample { Value = new[] { 1, 2 } };
			var changed = new Sample { Value = new[] { 1, 2, 3 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void ClassUnchanged()
		{
			const string diff = "";
			var @base = new Sample { Value = new[] { 1, 2, 3 } };
			var changed = new Sample { Value = new[] { 1, 2, 3 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void DictionaryOneAdded()
		{
			const string diff =
				"=a:\r\n" +
				"\t+2:3";
			var @base = new Dictionary<string, int[]> { { "a", new[] { 1, 2 } } };
			var changed = new Dictionary<string, int[]> { { "a", new[] { 1, 2, 3 } } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void DictionaryUnchanged()
		{
			const string diff = "";
			var @base = new Dictionary<string, int[]> { { "a", new[] { 1, 2, 3 } } };
			var changed = new Dictionary<string, int[]> { { "a", new[] { 1, 2, 3 } } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
