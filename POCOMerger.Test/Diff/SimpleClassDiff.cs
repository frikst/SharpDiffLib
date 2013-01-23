using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.diff;
using POCOMerger.definition;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class SimpleClassDiff
	{
		private class Sample
		{
			public string Value { get; set; }
			public string Value2 { get; set; }

			public override string ToString()
			{
				return "<Sample>";
			}
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.ClassDiffRules();
			}
		}

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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
