using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
		private Func<TType, TType, IDiff<TType>> aCallComputeInternal;

		public SortedCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aCallComputeInternal == null)
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

				ParameterExpression @baseParameter = Expression.Parameter(typeof(TType), "base");
				ParameterExpression changedParameter = Expression.Parameter(typeof(TType), "changed");

				Expression<Func<TType, TType, IDiff<TType>>> callComputeExpression =
					Expression.Lambda<Func<TType, TType, IDiff<TType>>>(
						Expression.Call(Expression.Constant(this), computeInternal, @baseParameter, changedParameter),
						@baseParameter, changedParameter
					);

				this.aCallComputeInternal = callComputeExpression.Compile();
			}

			return this.aCallComputeInternal(@base, changed);
		}

		#endregion

		private IDiff<TType> ComputeInternal<TItemType>(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			throw new NotImplementedException();
		}
	}
}
