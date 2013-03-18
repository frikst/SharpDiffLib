using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.applyPatch.common.value
{
	public class ApplyValuePatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ApplyValuePatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
