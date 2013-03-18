using System;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.mergeDiffs.collection.keyValue
{
	public class MergeKeyValueCollectionDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		public IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>()
		{
			if (Class<TType>.KeyValueParams == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			return (IMergeDiffsAlgorithm<TType>)Activator.CreateInstance(
				typeof(MergeKeyValueCollectionDiffs<,,>).MakeGenericType(
					typeof(TType),
					Class<TType>.KeyValueParams[0],
					Class<TType>.KeyValueParams[1]
				),
				this.MergerImplementation
			);
		}

		#endregion
	}
}
