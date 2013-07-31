using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.resolveConflicts.common.dontResolve
{
	public class DontResolveRules<TDefinedFor> : BaseRules<TDefinedFor>, IResolveConflictsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IAlgorithmRules

		IResolveConflictsAlgorithm<TType> IResolveConflictsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new DontResolve<TType>();
		}

		#endregion
	}
}