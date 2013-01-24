﻿using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;

namespace POCOMerger.algorithms.applyPatch.common.@class
{
	public class ApplyClassPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		public IApplyPatchAlgorithm<TType> GetAlgorithm<TType>()
		{
			return new ApplyClassPatch<TType>(this.MergerImplementation);
		}

		#endregion
	}
}