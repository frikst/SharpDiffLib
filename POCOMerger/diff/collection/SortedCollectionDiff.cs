using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.diff.collection
{
	public class SortedCollectionDiff<TType> : IDiffAlgorithm<TType>
	{
		private MergerImplementation aMergerImplementation;

		private Type aItemType;

		private Func<TType, TType, IDiff<TType>> aCallComputeInternal;

		public SortedCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aItemType = null;

			this.aMergerImplementation = mergerImplementation;

			foreach (Type @interface in typeof(TType).GetInterfaces())
			{
				if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					this.aItemType = @interface.GetGenericArguments()[0];
				}
			}

			if (this.aItemType == null)
				throw new Exception("Cannot compare non-collection type with SortedCollectionDiff");

			this.aCallComputeInternal = null;
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aCallComputeInternal == null)
				this.aCallComputeInternal = this.CompileCallComputeInternal();

			return this.aCallComputeInternal(@base, changed);
		}

		#endregion

		private Func<TType, TType, IDiff<TType>> CompileCallComputeInternal()
		{

			MethodInfo computeInternal =
				this.GetType()
					.GetMethod("ComputeInternal", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(this.aItemType);

			ParameterExpression baseParameter = Expression.Parameter(typeof(TType), "base");
			ParameterExpression changedParameter = Expression.Parameter(typeof(TType), "changed");

			Expression<Func<TType, TType, IDiff<TType>>> callComputeExpression =
				Expression.Lambda<Func<TType, TType, IDiff<TType>>>(
					Expression.Call(Expression.Constant(this), computeInternal, baseParameter, changedParameter),
					baseParameter, changedParameter
					);

			return callComputeExpression.Compile();
		}

		private Func<TItemType, TItemType, bool> CompileIsTheSame<TItemType>()
		{
			IEqualityComparer<TItemType> comparer = EqualityComparer<TItemType>.Default;

			return comparer.Equals;
		}

		private IDiff<TType> ComputeInternal<TItemType>(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			Func<TItemType, TItemType, bool> isTheSame = this.CompileIsTheSame<TItemType>();

			IEnumerator<TItemType> baseEnumerator = @base.GetEnumerator();
			Queue<TItemType> baseQueue = new Queue<TItemType>();
			IEnumerator<TItemType> changedEnumerator = changed.GetEnumerator();
			Queue<TItemType> changedQueue = new Queue<TItemType>();

			List<IDiffItem> ret = new List<IDiffItem>(20); // 20 seems to be a good value :)

			List<TItemType> baseQueueNew = new List<TItemType>(); // same here
			List<TItemType> changedQueueNew = new List<TItemType>();

			int index = 0;

			for (;;)
			{
				TItemType baseItem;
				bool baseHasItem = this.GetOne(baseEnumerator, baseQueue, out baseItem);
				TItemType changedItem;
				bool changedHasItem = this.GetOne(changedEnumerator, changedQueue, out changedItem);

				if (baseHasItem && changedHasItem)
				{
					if (isTheSame(baseItem, changedItem))
						index++;
					else
					{
						// optimization: do not create collections again, just clear it
						baseQueueNew.Clear();
						changedQueueNew.Clear();

						baseQueueNew.Add(baseItem);
						changedQueueNew.Add(changedItem);

						for (;;)
						{
							baseHasItem = this.GetOne(baseEnumerator, baseQueue, out baseItem);
							changedHasItem = this.GetOne(changedEnumerator, changedQueue, out changedItem);

							if (baseHasItem)
								baseQueueNew.Add(baseItem);

							if (changedHasItem)
								changedQueueNew.Add(changedItem);

							if (!baseHasItem && !changedHasItem)
							{
								foreach (TItemType item in baseQueueNew)
									ret.Add(new DiffOrderedCollectionRemoved<TItemType>(index, item));
								baseQueueNew.Clear();

								foreach (TItemType item in changedQueueNew)
								{
									ret.Add(new DiffOrderedCollectionAdded<TItemType>(index, item));
									index++;
								}
								changedQueueNew.Clear();

								break;
							}

							int idxFromChanged = changedQueueNew.FindIndex(x => isTheSame(x, baseItem));
							int idxFromBase = baseQueueNew.FindIndex(x => isTheSame(x, changedItem));

							if (baseHasItem && idxFromChanged >= 0 && (idxFromChanged < idxFromBase || idxFromBase < 0))
							{
								foreach (TItemType item in baseQueueNew.Take(baseQueueNew.Count - 1))
									ret.Add(new DiffOrderedCollectionRemoved<TItemType>(index, item));
								baseQueueNew.Clear();
								baseQueueNew.Add(baseItem);

								foreach (TItemType item in changedQueueNew.Take(idxFromChanged))
								{
									ret.Add(new DiffOrderedCollectionAdded<TItemType>(index, item));
									index++;
								}
								changedQueueNew.RemoveRange(0, idxFromChanged);

								break;
							}

							if (changedHasItem && idxFromBase >= 0 && (idxFromBase <= idxFromChanged || idxFromChanged < 0))
							{
								foreach (TItemType item in baseQueueNew.Take(idxFromBase))
									ret.Add(new DiffOrderedCollectionRemoved<TItemType>(index, item));
								baseQueueNew.RemoveRange(0, idxFromBase);

								foreach (TItemType item in changedQueueNew.Take(changedQueueNew.Count - 1))
								{
									ret.Add(new DiffOrderedCollectionAdded<TItemType>(index, item));
									index++;
								}
								changedQueueNew.Clear();
								changedQueueNew.Add(changedItem);

								break;
							}
						}

						baseQueue = baseQueueNew.ToQueue();
						changedQueue = changedQueueNew.ToQueue();
					}
				}
				else if (baseHasItem)
					ret.Add(new DiffOrderedCollectionRemoved<TItemType>(index, baseItem));
				else if (changedHasItem)
				{
					ret.Add(new DiffOrderedCollectionAdded<TItemType>(index, changedItem));
					index++;
				}
				else
					break;
			}

			return new Diff<TType>(ret);
		}

		private bool GetOne<TItemType>(IEnumerator<TItemType> enumerator, Queue<TItemType> queue, out TItemType result)
		{
			if (queue.Count > 0)
			{
				result = queue.Dequeue();
				return true;
			}
			else if (enumerator.MoveNext())
			{
				result = enumerator.Current;
				return true;
			}
			else
			{
				result = default(TItemType);
				return false;
			}
		}
	}
}
