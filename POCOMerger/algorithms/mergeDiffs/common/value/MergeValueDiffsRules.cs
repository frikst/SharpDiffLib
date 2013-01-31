using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.mergeDiffs.common.value
{
	public class MergeValueDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
			return new MergeValueDiffs<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
