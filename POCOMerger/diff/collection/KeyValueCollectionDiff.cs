using System;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	internal class KeyValueCollectionDiff<TType, TKeyType, TValueType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		public KeyValueCollectionDiff(MergerImplementation mergerImplementation)
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
