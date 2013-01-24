using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.applyPatch.common.value
{
	public class ApplyValuePatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		public IApplyPatchAlgorithm<TType> GetAlgorithm<TType>()
		{
			return new ApplyValuePatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
