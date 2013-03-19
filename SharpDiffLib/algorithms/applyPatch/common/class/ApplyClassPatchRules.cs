using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.applyPatch.common.@class
{
	/// <summary>
	/// Rules for class patch application algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which are rules defined.</typeparam>
	public class ApplyClassPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new ApplyClassPatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
