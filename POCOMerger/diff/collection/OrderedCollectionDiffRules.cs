using System;
using System.Collections.Generic;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class OrderedCollectionDiffRules : BaseRules, IDiffAlgorithmRules
	{
		#region Implementation of IDiffAlgorithmRules

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


			return (IDiffAlgorithm<TType>) Activator.CreateInstance(typeof(OrderedCollectionDiff<,>).MakeGenericType(typeof(TType), itemType), this.MergerImplementation);
		}

		#endregion
	}
}
