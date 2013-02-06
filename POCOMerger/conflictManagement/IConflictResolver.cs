using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;

namespace POCOMerger.conflictManagement
{
	public interface IConflictResolver
	{
		void ResolveConflict(IDiffItemConflicted conflict, ResolveAction resolve);

		bool HasConflicts { get; }
	}

	public interface IConflictResolver<TType> : IConflictResolver
	{
		IDiff<TType> Original { get; }

		IDiff<TType> Resolved { get; }
	}

	public enum ResolveAction
	{
		UseLeft,
		UseRight,
		LeftThenRight,
		RightThenLeft
	}
}