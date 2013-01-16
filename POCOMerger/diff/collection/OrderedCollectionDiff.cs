using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.diff.collection
{
	internal class OrderedCollectionDiff<TType, TItemType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;

		private readonly Property aIDProperty;

		private Func<TItemType, TItemType, bool> aIsTheSame;
		private IDiffAlgorithm<TItemType> aItemDiff;

		public OrderedCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIsTheSame = null;

			this.aIDProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aIsTheSame == null)
			{
				this.aIsTheSame = IdHelpers.CompileIsTheSame<TItemType>(this.aIDProperty);
				this.aItemDiff = this.aMergerImplementation.Partial.GetDiffAlgorithm<TItemType>();
			}

			return this.ComputeInternal((IEnumerable<TItemType>) @base, (IEnumerable<TItemType>) changed);
		}

		#endregion

		private IDiff<TType> ComputeInternal(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{

			Func<TItemType, TItemType, bool> isTheSame = this.aIsTheSame;

			IEnumerator<TItemType> baseEnumerator = @base.GetEnumerator();
			Queue<TItemType> baseQueue = new Queue<TItemType>();
			IEnumerator<TItemType> changedEnumerator = changed.GetEnumerator();
			Queue<TItemType> changedQueue = new Queue<TItemType>();

			List<IDiffItem> ret = new List<IDiffItem>(20); // 20 seems to be a good value :)

			List<TItemType> baseQueueNew = new List<TItemType>();
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
					{
						if (this.aIDProperty != null && this.aItemDiff != null)
						{
							IDiff itemDiff = this.aItemDiff.Compute(baseItem, changedItem);

							if (itemDiff.Count > 0)
								ret.Add(new DiffOrderedCollectionChanged<TItemType>(index, itemDiff));
						}

						index++;
					}
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
