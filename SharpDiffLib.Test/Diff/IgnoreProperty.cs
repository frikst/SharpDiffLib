﻿using System;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Test._Entities.BaseWithoutId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
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

		[Test]
		public void TestIgnoredDifferent()
		{
			string diff = "";

			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "two", Value2 = "three" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestProcessedDifferent()
		{
			string diff =
				"-Value2:three" + Environment.NewLine +
				"+Value2:four";

			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "one", Value2 = "four" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestIgnoredDifferentDescendant()
		{
			string diff = "";

			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "two", Value2 = "three", Value3 = "six", Value4 = "seven" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestProcessedDifferentDescendant()
		{
			string diff =
				"-Value2:three" + Environment.NewLine +
				"+Value2:four" + Environment.NewLine +
				"-Value4:seven" + Environment.NewLine +
				"+Value4:eight";

			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "one", Value2 = "four", Value3 = "five", Value4 = "eight" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
