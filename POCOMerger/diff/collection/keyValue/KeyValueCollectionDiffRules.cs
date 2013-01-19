using System;
using System.Collections.Generic;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

namespace POCOMerger.diff.collection.keyValue
{
	public class KeyValueCollectionDiffRules : BaseRules, IDiffAlgorithmRules
	{
		#region Implementation of IDiffAlgorithmRules

		public IDiffAlgorithm<TType> GetAlgorithm<TType>()
		{
			Type keyType = null;
			Type itemType = null;

			foreach (Type @interface in typeof(TType).GetInterfaces())
			{
				if (IsEnumerableOfKeyValuePair<TType>(@interface, out keyType, out itemType))
					break;
			}

			if (itemType == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");


			return (IDiffAlgorithm<TType>)Activator.CreateInstance(typeof(KeyValueCollectionDiff<,,>).MakeGenericType(typeof(TType), keyType, itemType), this.MergerImplementation);
		}

		private static bool IsEnumerableOfKeyValuePair<TType>(Type @interface, out Type keyType, out Type itemType)
		{
			if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				Type enumerableItemType = @interface.GetGenericArguments()[0];

				if (enumerableItemType.IsGenericType && enumerableItemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
				{
					Type[] keyValue = enumerableItemType.GetGenericArguments();
					keyType = keyValue[0];
					itemType = keyValue[1];

					return true;
				}
			}

			keyType = null;
			itemType = null;

			return false;
		}

		public override IEnumerable<Type> GetPossibleResults()
		{
			yield return typeof(IDiffKeyValueCollectionItem);
			yield return typeof(IDiffItemAdded);
			yield return typeof(IDiffItemRemoved);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}
