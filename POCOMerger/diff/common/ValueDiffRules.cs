using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.common
{
	public class ValueDiffRules : BaseRules, IDiffAlgorithmRules
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ValueDiff<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
