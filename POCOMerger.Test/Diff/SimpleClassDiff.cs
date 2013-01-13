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
		public void TestDifferent()
		{
			const string diff =
				"-Value:one\n" +
				"+Value:two";

			var ret = Merger.Instance.Partial.Diff(new Sample { Value = "one" }, new Sample { Value = "two" });

			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
