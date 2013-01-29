using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.applyPatch.collection.unordered
{
	internal class ApplyUnorderedCollectionWithIdPatch<TType, TIdType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Property aIdProperty;
		private Func<TItemType, TIdType> aIdAccessor;
		private IApplyPatchAlgorithm<TItemType> aApplyItemDiff;
		private Func<ICollection<TItemType>, TType> aConvertor;

		public ApplyUnorderedCollectionWithIdPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIdProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);

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
			if (this.aIdAccessor == null)
			{
				this.aIdAccessor = IdHelpers.CreateIdAccessor<TItemType, TIdType>(this.aIdProperty);
				this.aApplyItemDiff = this.aMergerImplementation.Partial.GetApplyPatchAlgorithm<TItemType>();
				this.aConvertor = this.CompileConvertor();
			}

			return this.ApplyInternal((IEnumerable<TItemType>) source, patch);
		}

		#endregion

		private Func<ICollection<TItemType>, TType> CompileConvertor()
		{
			if (typeof(TType).IsArray)
				return x => (TType)(object)x.ToArray();

			ParameterExpression from = Expression.Parameter(typeof(ICollection<TItemType>), "from");

			Expression<Func<ICollection<TItemType>, TType>> convertor = Expression.Lambda<Func<ICollection<TItemType>, TType>>(
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
			Dictionary<TIdType, TItemType> ret = source.ToDictionary(this.aIdAccessor);

			foreach (IDiffUnorderedCollectionItem item in patch)
			{
				if (item is IDiffItemAdded<TItemType>)
				{
					TIdType id;
					if (item is IDiffUnorderedCollectionItemWithID<TIdType>)
						id = ((IDiffUnorderedCollectionItemWithID<TIdType>) item).Id;
					else
						id = this.aIdAccessor(((IDiffItemAdded<TItemType>) item).NewValue);

					ret[id] = ((IDiffItemAdded<TItemType>) item).NewValue;
				}
				else if (item is IDiffItemRemoved<TItemType>)
				{
					TIdType id;
					if (item is IDiffUnorderedCollectionItemWithID<TIdType>)
						id = ((IDiffUnorderedCollectionItemWithID<TIdType>) item).Id;
					else
						id = this.aIdAccessor(((IDiffItemRemoved<TItemType>) item).OldValue);

					ret.Remove(id);
				}
				else if (item is IDiffItemReplaced<TItemType>)
				{
					TIdType idOld;
					TIdType idNew;
					if (item is IDiffUnorderedCollectionItemWithID<TIdType>)
						idNew = idOld = ((IDiffUnorderedCollectionItemWithID<TIdType>) item).Id;
					else
					{
						idOld = this.aIdAccessor(((IDiffItemReplaced<TItemType>) item).OldValue);
						idNew = this.aIdAccessor(((IDiffItemReplaced<TItemType>) item).NewValue);
					}

					ret.Remove(idOld);
					ret[idNew] = ((IDiffItemReplaced<TItemType>) item).NewValue;
				}
				else if (item is IDiffItemChanged)
				{
					TIdType id = ((IDiffUnorderedCollectionItemWithID<TIdType>)item).Id;

					ret[id] = this.aApplyItemDiff.Apply(
						ret[id],
						((IDiffItemChanged<TItemType>)item).ValueDiff
					);
				}
				else
					throw new InvalidOperationException();
			}

			return this.aConvertor(ret.Values);
		}
	}
}
