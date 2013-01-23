using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.diff;
using POCOMerger.definition;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class IgnoreProperty
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

		private class SampleDesc : Sample
		{
			public string Value3 { get; set; }
			public string Value4 { get; set; }

			public override string ToString()
			{
				return "<SampleDesc>";
			}
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.Inheritable.ClassDiffRules(rules => rules
						.Ignore(x => x.Value)
					);

				Define<SampleDesc>()
					.ClassDiffRules(rules => rules
						.Ignore(x => x.Value3)
					);
			}
		}

		[TestMethod]
		public void TestIgnoredDifferent()
		{
			const string diff = "";

			var @base = new Sample { Value = "one", Value2 = "three" };
			var changed = new Sample { Value = "two", Value2 = "three" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestProcessedDifferent()
		{
			const string diff =
				"-Value2:three\r\n" +
				"+Value2:four";

			var @base = new Sample { Value = "one", Value2 = "three" };
			var changed = new Sample { Value = "one", Value2 = "four" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestIgnoredDifferentDescendant()
		{
			const string diff = "";

			var @base = new SampleDesc { Value = "one", Value2 = "three", Value3 = "fife", Value4 = "seven" };
			var changed = new SampleDesc { Value = "two", Value2 = "three", Value3 = "six", Value4 = "seven" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestProcessedDifferentDescendant()
		{
			const string diff =
				"-Value2:three\r\n" +
				"+Value2:four\r\n" +
				"-Value4:seven\r\n" +
				"+Value4:eight";

			var @base = new SampleDesc { Value = "one", Value2 = "three", Value3 = "fife", Value4 = "seven" };
			var changed = new SampleDesc { Value = "one", Value2 = "four", Value3 = "fife", Value4 = "eight" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
