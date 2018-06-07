using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class SetOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<int>>()
					.UnorderedCollectionDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			string diff =
				"+3";
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoAdded()
		{
			string diff =
				"+3" + Environment.NewLine +
				"+4";
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3, 4 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoRemoved()
		{
			string diff =
				"-3" + Environment.NewLine +
				"-4";
			var @base = new HashSet<int> { 1, 2, 3, 4 };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllAdded()
		{
			string diff =
				"+1" + Environment.NewLine +
				"+2";
			var @base = new HashSet<int> { };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllRemoved()
		{
			string diff =
				"-1" + Environment.NewLine +
				"-2";
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void UnchangedEmpty()
		{
			string diff = "";
			var @base = new HashSet<int> { };
			var changed = new HashSet<int> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void UnchangedNonEmpty()
		{
			string diff = "";
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
