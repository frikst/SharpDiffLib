using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class KeyValueCollectionDiffRules : IDiffAlgorithmRules
	{
		private MergerImplementation aMergerImplementation;

		public KeyValueCollectionDiffRules()
		{
			this.aMergerImplementation = null;
		}

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


			return (IDiffAlgorithm<TType>)Activator.CreateInstance(typeof(KeyValueCollectionDiff<,,>).MakeGenericType(typeof(TType), keyType, itemType), this.aMergerImplementation);
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

		#endregion

		#region Implementation of IAlgorithmRules

		public void Initialize(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#endregion
	}
}
