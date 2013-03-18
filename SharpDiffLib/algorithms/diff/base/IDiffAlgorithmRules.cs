using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.diff.@base
{
	public interface IDiffAlgorithmRules : IAlgorithmRules
	{
		IDiffAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IDiffAlgorithmRules<TDefinedFor> : IDiffAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{
		
	}
}
