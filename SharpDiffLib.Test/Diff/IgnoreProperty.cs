﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.BaseWithoutId;
using SharpDiffLib.algorithms.diff;
using SharpDiffLib.definition;

namespace SharpDiffLib.Test.Diff
{
	[TestClass]
	public class IgnoreProperty
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.Inheritable.ClassDiffRules(rules => rules
						.Ignore(x => x.Value)
					);

				Define<SampleDescendant>()
					.ClassDiffRules(rules => rules
						.Ignore(x => x.Value3)
					);
			}
		}

		[TestMethod]
		public void TestIgnoredDifferent()
		{
			const string diff = "";

			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "two", Value2 = "three" };

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

			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "one", Value2 = "four" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestIgnoredDifferentDescendant()
		{
			const string diff = "";

			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "two", Value2 = "three", Value3 = "six", Value4 = "seven" };

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

			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "one", Value2 = "four", Value3 = "five", Value4 = "eight" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}