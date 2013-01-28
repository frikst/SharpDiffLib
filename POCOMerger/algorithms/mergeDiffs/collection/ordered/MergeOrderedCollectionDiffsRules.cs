using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.definition.rules;
using POCOMerger.fastReflection;

namespace POCOMerger.algorithms.mergeDiffs.collection.ordered
{
	public class MergeOrderedCollectionDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		public IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>()
		{
			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");


			return (IMergeDiffsAlgorithm<TType>)Activator.CreateInstance(
				typeof(MergeOrderedCollectionDiffs<,>).MakeGenericType(
					typeof(TType),
					Class<TType>.EnumerableParam
				),
				this.MergerImplementation
			);
		}

		#endregion
	}
}
