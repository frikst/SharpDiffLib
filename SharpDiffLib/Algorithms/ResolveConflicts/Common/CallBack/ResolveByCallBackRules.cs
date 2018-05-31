using System;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.CallBack
{
	public class ResolveByCallBackRules<TDefinedFor> : BaseRules<TDefinedFor>, IResolveConflictsAlgorithmRules<TDefinedFor>
	{
		private Action<Type, IConflictResolver> aCallBack;

		public ResolveByCallBackRules()
		{
			this.aCallBack = null;
		}

		public ResolveByCallBackRules<TDefinedFor> CallBack(Action<Type, IConflictResolver> callBack)
		{
			this.aCallBack = callBack;

			return this;
		}

		#region Implementation of IResolveConflictsAlgorithmRules

		IResolveConflictsAlgorithm<TType> IResolveConflictsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new ResolveByCallBack<TType>(this.aCallBack);
		}

		#endregion
	}
}
