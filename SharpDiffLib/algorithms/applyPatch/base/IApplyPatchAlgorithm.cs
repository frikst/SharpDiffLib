using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.algorithms.applyPatch.@base
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
