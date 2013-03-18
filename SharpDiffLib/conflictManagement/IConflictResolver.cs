using System.Collections.Generic;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;

namespace POCOMerger.conflictManagement
{
	public interface IConflictResolver : IEnumerable<IDiffItemConflicted>
	{
		void ResolveConflict(IDiffItemConflicted conflict, ResolveAction resolve);

		bool HasConflicts { get; }
	}

	public interface IConflictResolver<TType> : IConflictResolver
	{
		IDiff<TType> Original { get; }

		IDiff<TType> Resolved { get; }

		IDiff<TType> Finish();
	}
}