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

	public interface IConflictResolver<out TType> : IConflictResolver
	{
		new IDiff<TType> Original { get; }

		new IDiff<TType> Resolved { get; }

		IDiff<TType> Finish();
	}
}