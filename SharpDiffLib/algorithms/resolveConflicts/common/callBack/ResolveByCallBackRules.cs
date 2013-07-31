using System;
using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.resolveConflicts.common.callBack
{
	public class ResolveByCallBackRules<TDefinedFor> : BaseRules<TDefinedFor>, IResolveConflictsAlgorithmRules<TDefinedFor>
	{
		private Action<IConflictResolver> aCallBack;

		public ResolveByCallBackRules()
		{
			this.aCallBack = null;
		}

		public ResolveByCallBackRules<TDefinedFor> CallBack(Action<IConflictResolver> callBack)
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
