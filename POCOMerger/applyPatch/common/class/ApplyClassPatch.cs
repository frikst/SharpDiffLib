using System;
using POCOMerger.applyPatch.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.implementation;

namespace POCOMerger.applyPatch.common.@class
{
	internal class ApplyClassPatch<TType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		public ApplyClassPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IApplyPatchAlgorithm<TType>

		public TType Apply(TType source, IDiff<TType> patch)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}