using System;
using System.Collections.Generic;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;

namespace POCOMerger.diff.collection.ordered
{
	public class OrderedCollectionDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");


			return (IDiffAlgorithm<TType>) Activator.CreateInstance(
				typeof(OrderedCollectionDiff<,>).MakeGenericType(
					typeof(TType),
					Class<TType>.EnumerableParam
				),
				this.MergerImplementation
			);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffOrderedCollectionItem);
			yield return typeof(IDiffItemAdded);
			yield return typeof(IDiffItemRemoved);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}
