using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
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
			const string diff =
				"-Value:one\r\n" +
				"+Value:two";

			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "two", Value2 = "three" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestIncludedSame()
		{
			const string diff =
				" Value:one\r\n" +
				"-Value2:three\r\n" +
				"+Value2:four";

			var @base = new SampleBase { Value = "one", Value2 = "three" };
			var changed = new SampleBase { Value = "one", Value2 = "four" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestIncludedDifferentDescendant()
		{
			const string diff =
				"-Value:one\r\n" +
				"+Value:two\r\n" +
				"-Value3:five\r\n" +
				"+Value3:six";

			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "two", Value2 = "three", Value3 = "six", Value4 = "seven" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestIncludedSameDescendant()
		{
			const string diff =
				" Value:one\r\n" +
				"-Value2:three\r\n" +
				"+Value2:four\r\n" +
				" Value3:five\r\n" +
				"-Value4:seven\r\n" +
				"+Value4:eight";

			var @base = new SampleDescendant { Value = "one", Value2 = "three", Value3 = "five", Value4 = "seven" };
			var changed = new SampleDescendant { Value = "one", Value2 = "four", Value3 = "five", Value4 = "eight" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(4, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
