using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class SimpleValue
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int>()
					.ApplyValuePatchRules();
			}
		}

		[Test]
		public void TestIntDifferent()
		{
			var diff = DiffFactory.Create<int>().Value()
			    .Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.ApplyPatch(5, diff);

			Assert.AreEqual(0, ret);
		}

		[Test]
		public void TestSameValues()
		{
			var diff = DiffFactory.Create<int>().Value()
				.MakeDiff();

			var ret = Merger.Instance.Partial.ApplyPatch(5, diff);

			Assert.AreEqual(5, ret);
		}
	}
}
