using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Common.Class
{
	/// <summary>
	/// Rules for class patch application algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
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
