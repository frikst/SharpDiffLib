using POCOMerger.diffResult.@base;

namespace POCOMerger.applyPatch.@base
{
	public interface IApplyPatchAlgorithm
	{

	}

	public interface IApplyPatchAlgorithm<TType> : IApplyPatchAlgorithm
	{
		TType Apply(TType source, IDiff<TType> patch);
	}
}
