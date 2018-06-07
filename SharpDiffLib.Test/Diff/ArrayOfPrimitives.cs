using System;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
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

		[Test]
		public void OneAdded()
		{
			string diff =
				"+2:3";
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoAdded()
		{
			string diff =
				"+2:2" + Environment.NewLine +
				"+2:3";
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void ManyAdded()
		{
			string diff =
				"+0:2" + Environment.NewLine +
				"+1:2" + Environment.NewLine +
				"+2:2" + Environment.NewLine +
				"+3:2" + Environment.NewLine +
				"+4:2";
			var @base = new[] { 1, 1, 1, 1 };
			var changed = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void OneRemoved()
		{
			string diff =
				"-2:3";
			var @base = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoRemoved()
		{
			string diff =
				"-2:2" + Environment.NewLine +
				"-3:3";
			var @base = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void ManyRemoved()
		{
			string diff =
				"-0:2" + Environment.NewLine +
				"-2:2" + Environment.NewLine +
				"-4:2" + Environment.NewLine +
				"-6:2" + Environment.NewLine +
				"-8:2";
			var @base = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 1, 1, 1, 1 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void OneReplaced()
		{
			string diff =
				"-2:3" + Environment.NewLine +
				"+3:4";
			var @base = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoReplaced()
		{
			string diff =
				"-2:2" + Environment.NewLine +
				"-3:3" + Environment.NewLine +
				"+4:4" + Environment.NewLine +
				"+4:5";
			var @base = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2, 4, 5 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(4, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void ManyReplaced()
		{
			string diff =
				"-0:2" + Environment.NewLine +
				"+1:3" + Environment.NewLine +
				"-2:2" + Environment.NewLine +
				"+3:3" + Environment.NewLine +
				"-4:2" + Environment.NewLine +
				"+5:3" + Environment.NewLine +
				"-6:2" + Environment.NewLine +
				"+7:3" + Environment.NewLine +
				"-8:2" + Environment.NewLine +
				"+9:3";
			var @base = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 3, 1, 3, 1, 3, 1, 3, 1, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(10, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllReplaced()
		{
			string diff =
				"-0:2" + Environment.NewLine +
				"-1:2" + Environment.NewLine +
				"-2:2" + Environment.NewLine +
				"-3:2" + Environment.NewLine +
				"-4:2" + Environment.NewLine +
				"+5:3" + Environment.NewLine +
				"+5:3" + Environment.NewLine +
				"+5:3" + Environment.NewLine +
				"+5:3" + Environment.NewLine +
				"+5:3";
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(10, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllAdded()
		{
			string diff =
				"+0:3" + Environment.NewLine +
				"+0:3" + Environment.NewLine +
				"+0:3" + Environment.NewLine +
				"+0:3" + Environment.NewLine +
				"+0:3";
			var @base = new int[] { };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllRemoved()
		{
			string diff =
				"-0:2" + Environment.NewLine +
				"-1:2" + Environment.NewLine +
				"-2:2" + Environment.NewLine +
				"-3:2" + Environment.NewLine +
				"-4:2";
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(5, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void EmptyUnchanged()
		{
			string diff = "";
			var @base = new int[] { };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void NonEmptyUnchanged()
		{
			string diff = "";
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 2, 2, 2, 2, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
