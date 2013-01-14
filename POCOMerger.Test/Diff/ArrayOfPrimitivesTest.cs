using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.diff.collection;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class ArrayOfPrimitivesTest
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.Rules<SortedCollectionDiffRules>();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+2:3";
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
