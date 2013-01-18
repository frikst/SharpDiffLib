using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class AlgorithmGuessing
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
				Define<Sample>();
			}
		}

		[TestMethod]
		public void TestInt()
		{
			const string diff =
				"-5\r\n" +
				"+0";

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestIntArray()
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
		public void TestIntSet()
		{
			const string diff =
				"+3";
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestDifferentObjects()
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
	}
}
