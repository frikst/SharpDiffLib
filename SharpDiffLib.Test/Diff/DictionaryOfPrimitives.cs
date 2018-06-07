using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
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

		[Test]
		public void OneAdded()
		{
			string diff =
				"+c:3";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoAdded()
		{
			string diff =
				"+c:3" + Environment.NewLine +
				"+d:4";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoRemoved()
		{
			string diff =
				"-c:3" + Environment.NewLine +
				"-d:4";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void OneReplaced()
		{
			string diff =
				"-c:3" + Environment.NewLine +
				"+c:4";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 4 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllAdded()
		{
			string diff =
				"+a:1" + Environment.NewLine +
				"+b:2";
			var @base = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllRemoved()
		{
			string diff =
				"-a:1" + Environment.NewLine +
				"-b:2";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void UnchangedEmpty()
		{
			string diff = "";
			var @base = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void UnchangedNonEmpty()
		{
			string diff = "";
			var @base = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
