using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.InnerClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class StructuredClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleInner>()
					.ApplyClassPatchRules();
				Define<Sample>()
					.ApplyClassPatchRules();
			}
		}

		[Test]
		public void EmptyPatch()
		{
			var diff = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("hello", ret.ValueInner.Value);
		}

		[Test]
		public void ReplaceGoodValue()
		{
			var diff = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.ValueInner, new SampleInner { Value = "hello" }, new SampleInner { Value = "world" })
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("world", ret.ValueInner.Value);
		}

		[Test]
		public void ChangeInner()
		{
			var diff = DiffFactory.Create<Sample>().Class()
				.Changed(x=>x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "hello", "world")
				)
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("world", ret.ValueInner.Value);
		}
	}
}
