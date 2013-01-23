using System;
using System.Collections.Generic;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;

namespace POCOMerger.algorithms.diff.collection.unordered
{
	public class UnorderedCollectionDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			Property idProperty = GeneralRulesHelper.GetIdProperty(this.MergerImplementation, Class<TType>.EnumerableParam);

			if (idProperty == null)
				return (IDiffAlgorithm<TType>) Activator.CreateInstance(
					typeof(UnorderedCollectionDiff<,>).MakeGenericType(
						typeof(TType),
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
			else
				return (IDiffAlgorithm<TType>) Activator.CreateInstance(
					typeof(UnorderedCollectionWithIdDiff<,,>).MakeGenericType(
						typeof(TType),
						idProperty.Type,
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffUnorderedCollectionItem);
			yield return typeof(IDiffUnorderedCollectionItemWithID);
			yield return typeof(IDiffItemAdded);
			yield return typeof(IDiffItemRemoved);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}
