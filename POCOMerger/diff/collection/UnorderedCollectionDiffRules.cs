using System;
using System.Collections.Generic;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class UnorderedCollectionDiffRules : IDiffAlgorithmRules
	{
		private MergerImplementation aMergerImplementation;

		public UnorderedCollectionDiffRules()
		{
			this.aMergerImplementation = null;
		}

		#region Implementation of IDiffAlgorithmRules

		public IDiffAlgorithm<TType> GetAlgorithm<TType>()
		{
			Type itemType = null;

			foreach (Type @interface in typeof(TType).GetInterfaces())
			{
				if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					itemType = @interface.GetGenericArguments()[0];
				}
			}

			if (itemType == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");


			return (IDiffAlgorithm<TType>)Activator.CreateInstance(typeof(UnorderedCollectionDiff<,>).MakeGenericType(typeof(TType), itemType), this.aMergerImplementation);
		}

		#endregion

		#region Implementation of IAlgorithmRules

		public void Initialize(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#endregion
	}
}
