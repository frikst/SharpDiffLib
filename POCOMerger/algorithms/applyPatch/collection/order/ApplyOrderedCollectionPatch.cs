using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.applyPatch.collection.order
{
	internal class ApplyOrderedCollectionPatch<TType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<List<TItemType>, TType> aConvertor;
		private IApplyPatchAlgorithm<TItemType> aApplyItemDiff;

		public ApplyOrderedCollectionPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aConvertor = null;
			this.aApplyItemDiff = null;
		}

		#region Implementation of IApplyPatchAlgorithm

		public object Apply(object source, IDiff patch)
		{
			return this.Apply((TType)source, (IDiff<TType>)patch);
		}

		#endregion

		#region Implementation of IApplyPatchAlgorithm<TType>

		public TType Apply(TType source, IDiff<TType> patch)
		{
			if (this.aConvertor == null)
			{
				this.aConvertor = this.CompileConvertor();
				this.aApplyItemDiff = this.aMergerImplementation.Partial.GetApplyPatchAlgorithm<TItemType>();
			}

			return this.ApplyInternal((IEnumerable<TItemType>)source, patch);
		}

		#endregion

		private Func<List<TItemType>, TType> CompileConvertor()
		{
			if (typeof(TType).IsGenericType
							&& (typeof(TType).GetGenericTypeDefinition() == typeof(List<>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(IEnumerable<>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(ICollection<>)))
				return x => (TType)(object)x;

			if (typeof(TType).IsArray)
				return x => (TType)(object)x.ToArray();

			ParameterExpression from = Expression.Parameter(typeof(List<TItemType>), "from");

			Expression<Func<List<TItemType>, TType>> convertor = Expression.Lambda<Func<List<TItemType>, TType>>(
				Expression.New(
					typeof(TType).GetConstructor(new[] { typeof(IEnumerable<TItemType>) }),
					Expression.Convert(from, typeof(IEnumerable<TItemType>))
				),
				from
			);

			return convertor.Compile();
		}

		private TType ApplyInternal(IEnumerable<TItemType> source, IDiff<TType> patch)
		{
			List<TItemType> ret = new List<TItemType>();

			IEnumerator<TItemType> enumerator = source.GetEnumerator();
			bool lastMoveNext = enumerator.MoveNext();
			int currentIndex = 0;

			foreach (IDiffOrderedCollectionItem item in patch)
			{
				while (currentIndex < item.ItemIndex)
				{
					ret.Add(enumerator.Current);
					lastMoveNext = enumerator.MoveNext();
					currentIndex++;
				}

				if (item is IDiffItemAdded<TItemType>)
					ret.Add(((IDiffItemAdded<TItemType>)item).NewValue);
				else if (item is IDiffItemChanged)
				{
					ret.Add(
						this.aApplyItemDiff.Apply(
							enumerator.Current,
							(IDiff<TItemType>)((IDiffItemChanged)item).ValueDiff
						)
					);
					lastMoveNext = enumerator.MoveNext();
				}
				else if (item is IDiffItemRemoved<TItemType>)
					lastMoveNext = enumerator.MoveNext();
				else if (item is IDiffItemReplaced<TItemType>)
				{
					lastMoveNext = enumerator.MoveNext();
					ret.Add(((IDiffItemReplaced<TItemType>)item).NewValue);
					currentIndex++;
				}
				else
					throw new InvalidOperationException();
			}

			while (lastMoveNext)
			{
				ret.Add(enumerator.Current);
				lastMoveNext = enumerator.MoveNext();
			}

			return this.aConvertor(ret);
		}
	}
}
