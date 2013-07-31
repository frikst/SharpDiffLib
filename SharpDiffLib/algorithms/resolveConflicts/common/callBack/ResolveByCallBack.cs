using System;
using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.conflictManagement;

namespace SharpDiffLib.algorithms.resolveConflicts.common.callBack
{
	internal class ResolveByCallBack<TType> : IResolveConflictsAlgorithm<TType>
	{
		private readonly Action<IConflictResolver> aCallBack;

		public ResolveByCallBack(Action<IConflictResolver> callBack)
		{
			this.aCallBack = callBack;
		}

		#region Implementation of IResolveConflictsAlgorithm

		public void ResolveConflicts(IConflictResolver<TType> resolver)
		{
			this.aCallBack(resolver);
		}

		public void ResolveConflicts(IConflictResolver resolver)
		{
			this.aCallBack(resolver);
		}

		#endregion
	}
}