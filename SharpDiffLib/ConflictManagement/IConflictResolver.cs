using System;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.ConflictManagement
{
	public interface IConflictResolver : IEnumerable<IDiffItemConflicted>
	{
		void ResolveConflict(IDiffItemConflicted conflict, ResolveAction resolve);

		bool HasConflicts { get; }

		IDiff Original { get; }

		IDiff Resolved { get; }

		Type ObjectType { get; }
	}

	public interface IConflictResolver<TType> : IConflictResolver
	{
		IDiff<TType> Original { get; }

		IDiff<TType> Resolved { get; }

		IDiff<TType> Finish();
	}
}