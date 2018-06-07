using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
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
			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "two", Value2 = "three" };

			var expected = DiffResultFactory.Class<SampleBase>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestProcessedDifferent()
		{
			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "one", Value2 = "four" };

			var expected = DiffResultFactory.Class<SampleBase>()
				.Replaced(x => x.Value2, "three", "four")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIgnoredDifferentDescendant()
		{
			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "two", Value2 = "three", Value3 = "six", Value4 = "seven" };

			var expected = DiffResultFactory.Class<SampleDescendant>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestProcessedDifferentDescendant()
		{
			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "one", Value2 = "four", Value3 = "five", Value4 = "eight" };

			var expected = DiffResultFactory.Class<SampleDescendant>()
				.Replaced(x => x.Value2, "three", "four")
				.Replaced(x => x.Value4, "seven", "eight")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
