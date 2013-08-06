﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.SimpleClass;
using SharpDiffLib.algorithms.applyPatch;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.ApplyPatch
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

			Assert.AreEqual("hello", ret.Value);
		}

		[TestMethod]
		public void ReplaceGoodValue()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "hello", "world")
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("world", ret.Value);
		}

		[TestMethod]
		public void Unchanged()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Unchanged(x => x.Value, "hello")
				.MakeDiff();

			Sample obj = new Sample { Value = "hello" };
			Sample ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("hello", ret.Value);
		}
	}
}
