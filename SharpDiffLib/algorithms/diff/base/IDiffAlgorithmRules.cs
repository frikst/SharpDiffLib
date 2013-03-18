using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.diff.@base
{
	public interface IDiffAlgorithmRules : IAlgorithmRules
	{
		IDiffAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IDiffAlgorithmRules<TDefinedFor> : IDiffAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{
		
	}
}
