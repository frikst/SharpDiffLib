using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.mergeDiffs.@base
{
	public interface IMergeDiffsAlgorithmRules : IAlgorithmRules
	{
		IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IMergeDiffsAlgorithmRules<TDefinedFor> : IMergeDiffsAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
