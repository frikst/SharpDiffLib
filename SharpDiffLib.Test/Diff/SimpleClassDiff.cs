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
			const string diff =
				"-Value:one\r\n" +
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
			const string diff =
				"-Value:one\r\n" +
				"+Value:two\r\n" +
				"-Value2:one2\r\n" +
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
			const string diff = "";

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
			const string diff =
				"-Value2:(null)\r\n" +
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
			const string diff =
				"-Value2:one2\r\n" +
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
			const string diff = "";

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
