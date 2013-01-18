using System;
using System.Collections.Generic;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

namespace POCOMerger.diff.common
{
	public class ValueDiffRules : BaseRules, IDiffAlgorithmRules
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ValueDiff<TType>(this.MergerImplementation);
		}

		public override IEnumerable<Type> GetPossibleResults()
		{
			yield return typeof(IDiffValue);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}
