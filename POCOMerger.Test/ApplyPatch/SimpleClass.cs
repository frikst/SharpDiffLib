using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.Test._Entities.SimpleClass;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.definition;
using POCOMerger.diffResult;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class SimpleClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.ApplyClassPatchRules();
			}
		}

		[TestMethod]
		public void EmptyPatch()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.Value, "hello");
		}

		[TestMethod]
		public void ReplaceGoodValue()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "hello", "world")
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.Value, "world");
		}
	}
}
