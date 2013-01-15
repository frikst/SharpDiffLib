using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.definition;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class OrderedCollectionDiffRules : IDiffAlgorithmRules
	{
		private MergerImplementation aMergerImplementation;

		public OrderedCollectionDiffRules()
		{
			this.aMergerImplementation = null;
		}

		#region Implementation of IAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
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


			return (IDiffAlgorithm<TType>) Activator.CreateInstance(typeof(OrderedCollectionDiff<,>).MakeGenericType(typeof(TType), itemType), this.aMergerImplementation);
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
