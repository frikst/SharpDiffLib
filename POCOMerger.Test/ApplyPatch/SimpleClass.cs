﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.definition;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class SimpleClass
	{
		private class Sample
		{
			public string Value { get; set; }
			public string Value2 { get; set; }

			public override string ToString()
			{
				return "<Sample>";
			}
		}

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
			IDiff<Sample> diff = DiffResultFactory<Sample>.Class.Create()
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.Value, "hello");
		}

		[TestMethod]
		public void ReplaceGoodValue()
		{
			IDiff<Sample> diff = DiffResultFactory<Sample>.Class.Create()
				.Replaced(x => x.Value, "hello", "world")
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual(ret.Value, "world");
		}
	}
}