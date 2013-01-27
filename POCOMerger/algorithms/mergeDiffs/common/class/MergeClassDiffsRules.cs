using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.mergeDiffs.common.@class
{
	public class MergeClassDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		public IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>()
		{
			return new MergeClassDiffs<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
