using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
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
			var expected = DiffResultFactory.Value<int>.Create()
				.Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestSameValues()
		{
			var expected = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(0, 0);

			Assert.AreEqual(expected, ret);
		}
	}
}
