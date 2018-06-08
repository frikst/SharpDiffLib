using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.BaseWithoutId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class AlwaysInclude
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.Inheritable.ClassDiffRules(rules => rules
						.AlwaysInclude(x => x.Value)
					);

				Define<SampleDescendant>()
					.ClassDiffRules(rules => rules
						.AlwaysInclude(x => x.Value3)
					);
			}
		}

		[Test]
		public void TestIncludedDifferent()
		{
			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "two", Value2 = "three" };

			var expected = DiffFactory.Create<SampleBase>().Class()
				.Replaced(x => x.Value, "one", "two")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIncludedSame()
		{
			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "one", Value2 = "four" };

			var expected = DiffFactory.Create<SampleBase>().Class()
				.Unchanged(x => x.Value, "one")
				.Replaced(x => x.Value2, "three", "four")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIncludedDifferentDescendant()
		{
			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "two", Value2 = "three", Value3 = "six", Value4 = "seven" };

			var expected = DiffFactory.Create<SampleDescendant>().Class()
				.Replaced(x => x.Value, "one", "two")
				.Replaced(x => x.Value3, "five", "six")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIncludedSameDescendant()
		{
			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "one", Value2 = "four", Value3 = "five", Value4 = "eight" };

			var expected = DiffFactory.Create<SampleDescendant>().Class()
				.Unchanged(x => x.Value, "one")
				.Replaced(x => x.Value2, "three", "four")
				.Unchanged(x => x.Value3, "five")
				.Replaced(x => x.Value4, "seven", "eight")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
