using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.Test._Entities.SimpleClass;
using POCOMerger.algorithms.mergeDiffs;
using POCOMerger.definition;
using POCOMerger.diffResult;

namespace POCOMerger.Test.MergeDiffs
{
	[TestClass]
	public class SimpleClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.MergeClassDiffsRules();
			}
		}

		[TestMethod]
		public void MergeEmptyDiffs()
		{
			var left = DiffResultFactory.Class<Sample>.Create()
			    .MakeDiff();
			var right = DiffResultFactory.Class<Sample>.Create()
				.MakeDiff();

			bool hadConflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out hadConflicts);

			var merged = DiffResultFactory.Class<Sample>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(hadConflicts);
		}
	}
}
