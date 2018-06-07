using System;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class SimpleClassDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.ClassDiffRules();
			}
		}

		[Test]
		public void TestOneDifferent()
		{
			string diff =
				"-Value:one" + Environment.NewLine +
				"+Value:two";

			var @base = new Sample { Value = "one" };
			var changed = new Sample { Value = "two" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestAllDifferent()
		{
			string diff =
				"-Value:one" + Environment.NewLine +
				"+Value:two" + Environment.NewLine +
				"-Value2:one2" + Environment.NewLine +
				"+Value2:two2";

			var @base = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};
			var changed = new Sample
			{
				Value = "two",
				Value2 = "two2"
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestNoneDifferent()
		{
			string diff = "";

			var @base = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestNullLeft()
		{
			string diff =
				"-Value2:(null)" + Environment.NewLine +
				"+Value2:one2";

			var @base = new Sample
			{
				Value = "one",
				Value2 = null
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestNullRight()
		{
			string diff =
				"-Value2:one2" + Environment.NewLine +
				"+Value2:(null)";

			var @base = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = null
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestNullBoth()
		{
			string diff = "";

			var @base = new Sample
			{
				Value = "one",
				Value2 = null
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = null
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
