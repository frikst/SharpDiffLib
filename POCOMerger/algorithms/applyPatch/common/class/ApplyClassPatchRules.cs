using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.applyPatch.common.@class
{
	public class ApplyClassPatchRules<TClass> : BaseRules<TClass>, IApplyPatchAlgorithmRules<TClass>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		public IApplyPatchAlgorithm<TType> GetAlgorithm<TType>()
		{
			return new ApplyClassPatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
