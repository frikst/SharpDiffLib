using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.applyPatch.common.value
{
	/// <summary>
	/// Rules for simple value patch application.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public class ApplyValuePatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new ApplyValuePatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
