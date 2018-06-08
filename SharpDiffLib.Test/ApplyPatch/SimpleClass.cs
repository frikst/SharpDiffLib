using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
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

		[Test]
		public void EmptyPatch()
		{
			var diff = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("hello", ret.Value);
		}

		[Test]
		public void ReplaceGoodValue()
		{
			var diff = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "hello", "world")
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("world", ret.Value);
		}

		[Test]
		public void Unchanged()
		{
			var diff = DiffFactory.Create<Sample>().Class()
				.Unchanged(x => x.Value, "hello")
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("hello", ret.Value);
		}
	}
}
