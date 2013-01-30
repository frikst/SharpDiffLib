using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.mergeDiffs.collection.unordered
{
	internal class MergeUnorderedCollectionDiffs<TType, TIdType, TItemType> : IMergeDiffsAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly EqualityComparer<TIdType> aComparer;
		private readonly Property aIdProperty;

		private Func<TItemType, TIdType> aIdAccessor;
		private IMergeDiffsAlgorithm<TItemType> aMergeItemsDiffs;

		public MergeUnorderedCollectionDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIdProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);
			this.aComparer = EqualityComparer<TIdType>.Default;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			if (this.aIdAccessor == null)
			{
				if (this.aIdProperty == null)
				{
					ParameterExpression obj = Expression.Parameter(typeof(TItemType), "obj");

					Expression<Func<TItemType, TIdType>> identityFunction = Expression.Lambda<Func<TItemType, TIdType>>(
						obj, obj
					);

					this.aIdAccessor = identityFunction.Compile();
				}
				else
					this.aIdAccessor = IdHelpers.CreateIdAccessor<TItemType, TIdType>(this.aIdProperty);

				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.GetMergeDiffsAlgorithm<TItemType>();
			}

			hadConflicts = false;

			Dictionary<TIdType, IDiffUnorderedCollectionItem> rightIndex = new Dictionary<TIdType, IDiffUnorderedCollectionItem>(right.Count);

			foreach (IDiffItem item in right)
			{
				if (item is IDiffUnorderedCollectionItemWithID)
					rightIndex[((IDiffUnorderedCollectionItemWithID<TIdType>)item).Id] = (IDiffUnorderedCollectionItem) item;
				else if (item is IDiffItemAdded)
					rightIndex[this.aIdAccessor(((IDiffItemAdded<TItemType>) item).NewValue)] = (IDiffUnorderedCollectionItem) item;
				else if (item is IDiffItemRemoved)
					rightIndex[this.aIdAccessor(((IDiffItemRemoved<TItemType>)item).OldValue)] = (IDiffUnorderedCollectionItem) item;
				else if (item is IDiffItemReplaced)
					rightIndex[this.aIdAccessor(((IDiffItemReplaced<TItemType>) item).OldValue)] = (IDiffUnorderedCollectionItem) item;
				else
					throw new Exception();
			}

			List<IDiffItem> ret = new List<IDiffItem>(left.Count + right.Count);

			foreach (IDiffUnorderedCollectionItem leftItem in left)
			{
				IDiffUnorderedCollectionItem rightItem;

				TIdType id;

				if (leftItem is IDiffUnorderedCollectionItemWithID)
					id = ((IDiffUnorderedCollectionItemWithID<TIdType>) leftItem).Id;
				else if (leftItem is IDiffItemAdded)
					id = this.aIdAccessor(((IDiffItemAdded<TItemType>) leftItem).NewValue);
				else if (leftItem is IDiffItemRemoved)
					id = this.aIdAccessor(((IDiffItemRemoved<TItemType>) leftItem).OldValue);
				else if (leftItem is IDiffItemReplaced)
					id = this.aIdAccessor(((IDiffItemReplaced<TItemType>) leftItem).OldValue);
				else
					throw new Exception();

				if (rightIndex.TryGetValue(id, out rightItem))
				{
					rightIndex.Remove(id);

					if (this.ProcessConflict(id, leftItem, rightItem, ret))
						hadConflicts = true;
				}
				else
					ret.Add(leftItem);
			}

			ret.AddRange(rightIndex.Values);

			return new Diff<TType>(ret);
		}

		#endregion

		private bool ProcessConflict(TIdType id, IDiffUnorderedCollectionItem leftItem, IDiffUnorderedCollectionItem rightItem, List<IDiffItem> ret)
		{
			if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
				return false;
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem);
				return false;
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				IDiff<TItemType> diffLeft = ((IDiffItemChanged<TItemType>) leftItem).ValueDiff;
				IDiff<TItemType> diffRight = ((IDiffItemChanged<TItemType>) rightItem).ValueDiff;

				bool hadConflicts;
				IDiff<TItemType> result = this.aMergeItemsDiffs.MergeDiffs(diffLeft, diffRight, out hadConflicts);

				ret.Add(new DiffUnorderedCollectionChanged<TIdType, TItemType>(id, result));

				return hadConflicts;
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
				return false;
			}

			ret.Add(new DiffAnyConflicted(leftItem, rightItem));
			return true;
		}
	}
}
