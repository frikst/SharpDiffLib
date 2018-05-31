using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.ConflictManagement;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.DontResolve
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
