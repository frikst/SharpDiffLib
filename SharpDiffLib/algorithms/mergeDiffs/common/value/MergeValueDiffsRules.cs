using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.mergeDiffs.common.value
{
	public class MergeValueDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
			return new MergeValueDiffs<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
