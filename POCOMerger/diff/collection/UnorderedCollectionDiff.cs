using System;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	internal class UnorderedCollectionDiff<TType, TItemType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		public UnorderedCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
