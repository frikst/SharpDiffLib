using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.mergeDiffs.common.@class
{
	public class MergeClassDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new MergeClassDiffs<TType>(this.MergerImplementation);
		}

		#endregion
	}
}
