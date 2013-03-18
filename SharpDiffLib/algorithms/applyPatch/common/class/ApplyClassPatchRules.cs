using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.applyPatch.common.@class
{
	public class ApplyClassPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ApplyClassPatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
