﻿using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.conflictManagement;

namespace SharpDiffLib.algorithms.resolveConflicts.common.dontResolve
{
	internal class DontResolve<TType> : IResolveConflictsAlgorithm<TType>
	{
		#region Implementation of IResolveConflictsAlgorithm<TType>

		public void ResolveConflicts(IConflictResolver<TType> resolver)
		{
			throw new NotPossibleToResolveException();
		}

		#endregion

		#region Implementation of IResolveConflictsAlgorithm

		public void ResolveConflicts(IConflictResolver resolver)
		{
			this.ResolveConflicts((IConflictResolver<TType>)resolver);
		}

		#endregion
	}
}