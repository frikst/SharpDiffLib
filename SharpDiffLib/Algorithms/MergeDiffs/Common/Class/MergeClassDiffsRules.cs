using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Common.Class
{
	public class MergeClassDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new MergeClassDiffs<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
