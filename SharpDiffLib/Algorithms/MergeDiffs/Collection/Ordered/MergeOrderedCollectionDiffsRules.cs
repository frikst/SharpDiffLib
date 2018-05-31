using System;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Ordered
{
	public class MergeOrderedCollectionDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

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
