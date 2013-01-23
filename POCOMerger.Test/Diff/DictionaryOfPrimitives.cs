using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.diff;
using POCOMerger.definition;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class DictionaryOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Dictionary<string, int>>()
					.KeyValueCollectionDiffRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+c:3";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoAdded()
		{
			const string diff =
				"+c:3\r\n" +
				"+d:4";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoRemoved()
		{
			const string diff =
				"-c:3\r\n" +
				"-d:4";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneReplaced()
		{
			const string diff =
				"-c:3\r\n" +
				"+c:4";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 4 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void AllAdded()
		{
			const string diff =
				"+a:1\r\n" +
				"+b:2";
			var @base = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void AllRemoved()
		{
			const string diff =
				"-a:1\r\n" +
				"-b:2";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void UnchangedEmpty()
		{
			const string diff = "";
			var @base = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void UnchangedNonEmpty()
		{
			const string diff = "";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
