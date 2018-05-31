using System;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.KeyValue
{
	public class MergeKeyValueCollectionDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		public IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

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
