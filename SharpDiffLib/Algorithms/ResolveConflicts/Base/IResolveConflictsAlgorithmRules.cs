using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts.Base
{
	public interface IResolveConflictsAlgorithmRules : IAlgorithmRules
	{
		IResolveConflictsAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IResolveConflictsAlgorithmRules<TDefinedFor> : IResolveConflictsAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
