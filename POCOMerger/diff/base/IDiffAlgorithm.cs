using POCOMerger.diffResult.@base;

namespace POCOMerger.diff.@base
{
	public interface IDiffAlgorithm
	{
		bool IsDirect { get; }
	}

	public interface IDiffAlgorithm<TType> : IDiffAlgorithm
	{
		IDiff<TType> Compute(TType @base, TType changed);
	}
}