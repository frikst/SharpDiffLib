using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.resolveConflicts.@base
{
	public interface IResolveConflictsAlgorithmRules : IAlgorithmRules
	{
		IResolveConflictsAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IResolveConflictsAlgorithmRules<TDefinedFor> : IResolveConflictsAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
