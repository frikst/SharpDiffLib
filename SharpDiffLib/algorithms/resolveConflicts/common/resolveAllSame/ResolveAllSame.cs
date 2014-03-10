using System.Linq;
using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.conflictManagement;

namespace SharpDiffLib.algorithms.resolveConflicts.common.resolveAllSame
{
	internal class ResolveAllSame<TType> : IResolveConflictsAlgorithm<TType>
	{
		private readonly ResolveAction aAction;

		public ResolveAllSame(ResolveAction action)
		{
			this.aAction = action;
		}

		#region Implementation of IResolveConflictsAlgorithm<TType>

		public void ResolveConflicts(IConflictResolver<TType> resolver)
		{
			foreach (var item in resolver.ToList())
				resolver.ResolveConflict(item, this.aAction);
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