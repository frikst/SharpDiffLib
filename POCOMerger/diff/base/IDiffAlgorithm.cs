using POCOMerger.diffResult.@base;

namespace POCOMerger.diff.@base
{
	public interface IDiffAlgorithm
	{
	}

	public interface IDiffAlgorithm<TType> : IDiffAlgorithm
	{
		IDiff<TType> Compute(TType @base, TType changed);
	}
}