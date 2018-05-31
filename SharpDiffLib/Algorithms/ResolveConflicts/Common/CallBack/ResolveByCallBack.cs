using System;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.ConflictManagement;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.CallBack
{
	internal class ResolveByCallBack<TType> : IResolveConflictsAlgorithm<TType>
	{
		private readonly Action<Type, IConflictResolver> aCallBack;

		public ResolveByCallBack(Action<Type, IConflictResolver> callBack)
		{
			this.aCallBack = callBack;
		}

		#region Implementation of IResolveConflictsAlgorithm<TType>

		public void ResolveConflicts(IConflictResolver<TType> resolver)
		{
			this.aCallBack(typeof(TType), resolver);
		}

		#endregion

		#region Implementation of IResolveConflictsAlgorithm

		public void ResolveConflicts(IConflictResolver resolver)
		{
			this.ResolveConflicts((IConflictResolver<TType>) resolver);
		}

		#endregion
	}
}