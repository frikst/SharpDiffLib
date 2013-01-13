using POCOMerger.definition;

namespace POCOMerger.diff.@base
{
	public interface IDiffAlgorithmRules : IAlgorithmRules
	{
		IDiffAlgorithm<TType> GetAlgorithm<TType>();
	}
}
