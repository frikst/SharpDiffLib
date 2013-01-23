using System;
using System.Collections.Generic;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

namespace POCOMerger.algorithms.diff.common.value
{
	public class ValueDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ValueDiff<TType>(this.MergerImplementation);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffValue);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}
