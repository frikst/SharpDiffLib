﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.algorithms.diff;
using SharpDiffLib.definition;

namespace SharpDiffLib.Test.Diff
{
	[TestClass]
	public class ValueNonObjectDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int>()
					.ValueDiffRules();
			}
		}

		[TestMethod]
		public void TestIntDifferent()
		{
			const string diff =
				"-5\r\n" +
				"+0";

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestSameValues()
		{
			const string diff = "";

			var ret = Merger.Instance.Partial.Diff(0, 0);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
