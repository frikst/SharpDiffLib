using KST.SharpDiffLib.ConflictManagement;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts.Base
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
