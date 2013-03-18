using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.mergeDiffs.@base
{
	public interface IMergeDiffsAlgorithmRules : IAlgorithmRules
	{
		IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IMergeDiffsAlgorithmRules<TDefinedFor> : IMergeDiffsAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
