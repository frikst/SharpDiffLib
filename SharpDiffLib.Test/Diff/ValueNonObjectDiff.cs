using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
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

		[Test]
		public void TestIntDifferent()
		{
			var expected = DiffFactory.Create<int>().Value()
				.Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestSameValues()
		{
			var expected = DiffFactory.Create<int>().Value()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(0, 0);

			Assert.AreEqual(expected, ret);
		}
	}
}
