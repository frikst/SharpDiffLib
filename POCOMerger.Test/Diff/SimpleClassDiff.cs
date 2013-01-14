using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.diff.common;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class SimpleClassDiff
	{
		private class Sample
		{
			public string Value { get; set; }
			public string Value2 { get; set; }
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.Rules<ClassDiffRules>();
			}
		}

		[TestMethod]
		public void TestOneDifferent()
		{
			const string diff =
				"-Value:one\r\n" +
				"+Value:two";

			var ret = Merger.Instance.Partial.Diff(new Sample { Value = "one" }, new Sample { Value = "two" });

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

			var ret = Merger.Instance.Partial.Diff(new Sample { Value = "one", Value2 = "one2" }, new Sample { Value = "two", Value2 = "two2" });

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestNoneDifferent()
		{
			var ret = Merger.Instance.Partial.Diff(new Sample { Value = "one", Value2 = "one2" }, new Sample { Value = "one", Value2 = "one2" });

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(string.Empty, ret.ToString());
		}
	}
}
