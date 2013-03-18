using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.algorithms.applyPatch;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.ApplyPatch
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
