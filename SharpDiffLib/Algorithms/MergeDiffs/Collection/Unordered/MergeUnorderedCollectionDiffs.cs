using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Implementation;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Unordered
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

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
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

				this.aMergeItemsDiffs = this.aMergerImplementation.Partial.Algorithms.GetMergeDiffsAlgorithm<TItemType>();
			}

			Dictionary<TIdType, IDiffUnorderedCollectionItem> rightIndex = new Dictionary<TIdType, IDiffUnorderedCollectionItem>(right.Count);

			foreach (IDiffItem item in right)
			{
				rightIndex[this.GetID(item)] = (IDiffUnorderedCollectionItem) item;
			}

			List<IDiffItem> ret = new List<IDiffItem>(left.Count + right.Count);

			foreach (IDiffUnorderedCollectionItem leftItem in left)
			{
				IDiffUnorderedCollectionItem rightItem;

				TIdType id = this.GetID(leftItem);

				if (rightIndex.TryGetValue(id, out rightItem))
				{
					rightIndex.Remove(id);

					this.ProcessConflict(id, leftItem, rightItem, ret, conflicts);
				}
				else
					ret.Add(leftItem);
			}

			ret.AddRange(rightIndex.Values);

			return new Diff<TType>(ret);
		}

		private TIdType GetID(IDiffItem leftItem)
		{
			if (leftItem is IDiffUnorderedCollectionItemWithID)
				return ((IDiffUnorderedCollectionItemWithID<TIdType>) leftItem).Id;
			else if (leftItem is IDiffItemAdded)
				return this.aIdAccessor(((IDiffItemAdded<TItemType>)leftItem).NewValue);
			else if (leftItem is IDiffItemRemoved)
				return this.aIdAccessor(((IDiffItemRemoved<TItemType>)leftItem).OldValue);
			else if (leftItem is IDiffItemReplaced)
				return this.aIdAccessor(((IDiffItemReplaced<TItemType>)leftItem).OldValue);
			else if (leftItem is IDiffItemUnchanged)
				return this.aIdAccessor(((IDiffItemUnchanged<TItemType>)leftItem).Value);
			else
				throw new Exception();
		}

		#endregion

		#region Implementation of IMergeDiffsAlgorithm

		IDiff IMergeDiffsAlgorithm.MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts)
		{
			return this.MergeDiffs((IDiff<TType>)left, (IDiff<TType>)right, conflicts);
		}

		#endregion

		private void ProcessConflict(TIdType id, IDiffUnorderedCollectionItem leftItem, IDiffUnorderedCollectionItem rightItem, List<IDiffItem> ret, IConflictContainer conflicts)
		{
			if (leftItem is IDiffItemAdded && rightItem is IDiffItemAdded && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemRemoved && rightItem is IDiffItemRemoved)
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemChanged && rightItem is IDiffItemChanged)
			{
				IDiff<TItemType> diffLeft = ((IDiffItemChanged<TItemType>) leftItem).ValueDiff;
				IDiff<TItemType> diffRight = ((IDiffItemChanged<TItemType>) rightItem).ValueDiff;

				IDiff<TItemType> result = this.aMergeItemsDiffs.MergeDiffs(diffLeft, diffRight, conflicts);

				ret.Add(new DiffUnorderedCollectionChanged<TIdType, TItemType>(id, result));
			}
			else if (leftItem is IDiffItemReplaced && rightItem is IDiffItemReplaced && leftItem.IsSame(rightItem))
			{
				ret.Add(leftItem);
			}
			else if (leftItem is IDiffItemUnchanged)
			{
				ret.Add(rightItem);
			}
			else if (rightItem is IDiffItemUnchanged)
			{
				ret.Add(leftItem);
			}
			else
			{
				DiffAnyConflicted conflict = new DiffAnyConflicted(leftItem, rightItem);
				ret.Add(conflict);
				conflicts.RegisterConflict(conflict);
			}
		}
	}
}
