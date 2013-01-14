using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.collection
{
	public class SortedCollectionDiff<TType> : IDiffAlgorithm<TType>
	{
		private MergerImplementation aMergerImplementation;

		public SortedCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
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
				throw new Exception("Cannot compare non-collection type with SortedCollectionDiff");

			MethodInfo computeInternal =
				this.GetType()
				    .GetMethod("ComputeInternal", BindingFlags.Instance | BindingFlags.NonPublic)
				    .MakeGenericMethod(itemType);

			return (IDiff<TType>) computeInternal.Invoke(this, new object[] { @base, changed });
		}

		#endregion

		private IDiff<IEnumerable<TItemType>> ComputeInternal<TItemType>(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			throw new NotImplementedException();
		}
	}
}
