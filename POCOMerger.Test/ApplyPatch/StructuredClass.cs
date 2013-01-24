using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.definition;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class StructuredClass
	{
		private class SampleInner
		{
			public string Value { get; set; }

			public override string ToString()
			{
				return "<SampleInner>";
			}
		}

		private class Sample
		{
			public SampleInner ValueInner { get; set; }

			public override string ToString()
			{
				return "<Sample>";
			}
		}

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

		[TestMethod]
		public void EmptyPatch()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.ValueInner.Value, "hello");
		}

		[TestMethod]
		public void ReplaceGoodValue()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.ValueInner, new SampleInner { Value = "hello" }, new SampleInner { Value = "world" })
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.ValueInner.Value, "world");
		}

		[TestMethod]
		public void ChangeInner()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Changed(x=>x.ValueInner, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "hello", "world")
					.MakeDiff()
				)
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.ValueInner.Value, "world");
		}
	}
}
