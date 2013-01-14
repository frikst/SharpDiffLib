using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.definition;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class SortedCollectionDiffRules : IDiffAlgorithmRules
	{
		private MergerImplementation aMergerImplementation;

		public SortedCollectionDiffRules()
		{
			this.aMergerImplementation = null;
		}

		#region Implementation of IAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new SortedCollectionDiff<TType>(this.aMergerImplementation);
		}

		#endregion

		#region Implementation of IAlgorithmRules

		void IAlgorithmRules.Initialize(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#endregion
	}
}
