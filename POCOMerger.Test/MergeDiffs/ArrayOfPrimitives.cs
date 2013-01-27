using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.mergeDiffs;
using POCOMerger.definition;

namespace POCOMerger.Test.MergeDiffs
{
	[TestClass]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.MergeOrderedCollectionDiffsRules();
			}
		}
	}
}
