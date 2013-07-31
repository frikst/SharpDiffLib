using SharpDiffLib.conflictManagement;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.algorithms.resolveConflicts.@base
{
	public interface IResolveConflictsAlgorithm
	{
		void ResolveConflicts(IConflictResolver resolver);
	}

	public interface IResolveConflictsAlgorithm<TType> : IResolveConflictsAlgorithm
	{
		void ResolveConflicts(IConflictResolver<TType> resolver);
	}
}
