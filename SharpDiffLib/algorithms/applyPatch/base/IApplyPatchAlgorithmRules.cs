using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.applyPatch.@base
{
	public interface IApplyPatchAlgorithmRules : IAlgorithmRules
	{
		IApplyPatchAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IApplyPatchAlgorithmRules<TDefinedFor> : IApplyPatchAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
