using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.resolveConflicts.common.resolveAllSame
{
	public class ResolveAllSameRules<TDefinedFor> : BaseRules<TDefinedFor>, IResolveConflictsAlgorithmRules<TDefinedFor>
	{
		private ResolveAction aAction;

		public ResolveAllSameRules()
		{
			this.aAction = ResolveAction.UseLeft;
		}

		public ResolveAllSameRules<TDefinedFor> Action(ResolveAction action)
		{
			this.aAction = action;

			return this;
		}

		#region Implementation of IResolveConflictsAlgorithmRules

		IResolveConflictsAlgorithm<TType> IResolveConflictsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new ResolveAllSame<TType>(this.aAction);
		}

		#endregion
	}
}
