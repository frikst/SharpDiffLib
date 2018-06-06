using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Implementation;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.Unordered
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
				this.aApplyItemDiff = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm<TItemType>();
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
				switch (item)
				{
					case IDiffItemAdded<TItemType> itemAdded:
						{
							TIdType id;
							if (itemAdded is IDiffUnorderedCollectionItemWithID<TIdType> itemWithID)
								id = itemWithID.Id;
							else
								id = this.aIdAccessor(itemAdded.NewValue);

							ret[id] = itemAdded.NewValue;
							break;
						}

					case IDiffItemRemoved<TItemType> itemRemoved:
						{
							TIdType id;
							if (itemRemoved is IDiffUnorderedCollectionItemWithID<TIdType> itemWithID)
								id = itemWithID.Id;
							else
								id = this.aIdAccessor(itemRemoved.OldValue);

							ret.Remove(id);
							break;
						}

					case IDiffItemReplaced<TItemType> itemReplaced:
						{
							TIdType idOld;
							TIdType idNew;
							if (itemReplaced is IDiffUnorderedCollectionItemWithID<TIdType> itemWithID)
								idNew = idOld = itemWithID.Id;
							else
							{
								idOld = this.aIdAccessor(itemReplaced.OldValue);
								idNew = this.aIdAccessor(itemReplaced.NewValue);
							}

							ret.Remove(idOld);
							ret[idNew] = itemReplaced.NewValue;
							break;
						}

					case IDiffItemChanged<TItemType> itemChanged:
						{
							TIdType id = ((IDiffUnorderedCollectionItemWithID<TIdType>)itemChanged).Id;

							ret[id] = this.aApplyItemDiff.Apply(ret[id], itemChanged.ValueDiff);
							break;
						}

					default:
						throw new InvalidOperationException();
				}
			}

			return this.aConvertor(ret.Values);
		}
	}
}
