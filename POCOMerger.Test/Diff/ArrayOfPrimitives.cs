using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.diff;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.OrderedCollectionDiffRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+2:3";
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoAdded()
		{
			const string diff =
				"+2:2\r\n" +
				"+2:3";
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void ManyAdded()
		{
			const string diff =
				"+0:2\r\n" +
				"+1:2\r\n" +
				"+2:2\r\n" +
				"+3:2\r\n" +
				"+4:2";
			var @base = new[] { 1, 1, 1, 1 };
			var changed = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneRemoved()
		{
			const string diff =
				"-2:3";
			var @base = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoRemoved()
		{
			const string diff =
				"-2:2\r\n" +
				"-2:3";
			var @base = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void ManyRemoved()
		{
			const string diff =
				"-0:2\r\n" +
				"-1:2\r\n" +
				"-2:2\r\n" +
				"-3:2\r\n" +
				"-4:2";
			var @base = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 1, 1, 1, 1 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneReplaced()
		{
			const string diff =
				"-2:3\r\n" +
				"+2:4";
			var @base = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoReplaced()
		{
			const string diff =
				"-2:2\r\n" +
				"-2:3\r\n" +
				"+2:4\r\n" +
				"+2:5";
			var @base = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2, 4, 5 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(4, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void ManyReplaced()
		{
			const string diff =
				"-0:2\r\n" +
				"+0:3\r\n" +
				"-1:2\r\n" +
				"+1:3\r\n" +
				"-2:2\r\n" +
				"+2:3\r\n" +
				"-3:2\r\n" +
				"+3:3\r\n" +
				"-4:2\r\n" +
				"+4:3";
			var @base = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 3, 1, 3, 1, 3, 1, 3, 1, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(10, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void AllReplaced()
		{
			const string diff =
				"-0:2\r\n" +
				"-0:2\r\n" +
				"-0:2\r\n" +
				"-0:2\r\n" +
				"-0:2\r\n" +
				"+0:3\r\n" +
				"+0:3\r\n" +
				"+0:3\r\n" +
				"+0:3\r\n" +
				"+0:3";
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(10, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void AllAdded()
		{
			const string diff =
				"+0:3\r\n" +
				"+0:3\r\n" +
				"+0:3\r\n" +
				"+0:3\r\n" +
				"+0:3";
			var @base = new int[] { };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void AllRemoved()
		{
			const string diff =
				"-0:2\r\n" +
				"-0:2\r\n" +
				"-0:2\r\n" +
				"-0:2\r\n" +
				"-0:2";
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void EmptyUnchanged()
		{
			const string diff = "";
			var @base = new int[] { };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void NonEmptyUnchanged()
		{
			const string diff = "";
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 2, 2, 2, 2, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
