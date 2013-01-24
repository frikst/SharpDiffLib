using POCOMerger.diffResult.@base;

namespace POCOMerger.algorithms.applyPatch.@base
{
	public interface IApplyPatchAlgorithm
	{
		object Apply(object source, IDiff patch);
	}

	public interface IApplyPatchAlgorithm<TType> : IApplyPatchAlgorithm
	{
		TType Apply(TType source, IDiff<TType> patch);
	}
}
