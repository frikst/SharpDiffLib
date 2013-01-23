using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.applyPatch.common.@class;
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
					.Rules<ApplyClassPatchRules<SampleInner>>();
				Define<Sample>()
					.Rules<ApplyClassPatchRules<Sample>>();
			}
		}

		[TestMethod]
		public void EmptyPatch()
		{
			IDiff<Sample> diff = DiffResultFactory<Sample>.Class.Create()
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.ValueInner.Value, "hello");
		}

		[TestMethod]
		public void ReplaceGoodValue()
		{
			IDiff<Sample> diff = DiffResultFactory<Sample>.Class.Create()
				.Replaced(x => x.ValueInner, new SampleInner { Value = "hello" }, new SampleInner { Value = "world" })
				.MakeDiff();

			Sample obj = new Sample { ValueInner = new SampleInner { Value = "hello" }, };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.ValueInner.Value, "world");
		}

		[TestMethod]
		public void ChangeInner()
		{
			IDiff<Sample> diff = DiffResultFactory<Sample>.Class.Create()
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
