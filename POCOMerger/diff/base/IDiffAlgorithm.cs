using POCOMerger.diffResult.@base;

namespace POCOMerger.diff.@base
{
	public interface IDiffAlgorithm
	{
		bool IsDirect { get; }

		IDiff Compute(object @base, object changed);
	}

	public interface IDiffAlgorithm<TType> : IDiffAlgorithm
	{
		IDiff<TType> Compute(TType @base, TType changed);
	}
}