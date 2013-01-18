using System;
using System.Collections.Generic;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class UnorderedCollectionDiffRules : BaseRules, IDiffAlgorithmRules
	{
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

			Property idProperty = GeneralRulesHelper.GetIdProperty(this.MergerImplementation, itemType);

			if (idProperty == null)
				return (IDiffAlgorithm<TType>) Activator.CreateInstance(
					typeof(UnorderedCollectionDiff<,>).MakeGenericType(typeof(TType), itemType),
					this.MergerImplementation
				);
			else
				return (IDiffAlgorithm<TType>) Activator.CreateInstance(
					typeof(UnorderedCollectionWithIdDiff<,,>).MakeGenericType(typeof(TType), idProperty.Type, itemType),
					this.MergerImplementation
				);
		}

		public override IEnumerable<Type> GetPossibleResults()
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
