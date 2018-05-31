using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestClass]
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

		[TestMethod]
		public void TestIntDifferent()
		{
			var diff = DiffResultFactory.Value<int>.Create()
			    .Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.ApplyPatch(5, diff);

			Assert.AreEqual(0, ret);
		}

		[TestMethod]
		public void TestSameValues()
		{
			var diff = DiffResultFactory.Value<int>.Create()
				.MakeDiff();

			var ret = Merger.Instance.Partial.ApplyPatch(5, diff);

			Assert.AreEqual(5, ret);
		}
	}
}
