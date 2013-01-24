using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.applyPatch.collection.unordered
{
	internal class ApplyUnorderedCollectionPatch<TType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private IEqualityComparer<TItemType> aItemComparer;
		private Func<HashSet<TItemType>, TType> aConvertor;

		public ApplyUnorderedCollectionPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
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
			if (this.aItemComparer == null)
			{
				this.aItemComparer = EqualityComparer<TItemType>.Default;
				this.aConvertor = this.CompileConvertor();
			}

			return this.ApplyInternal((IEnumerable<TItemType>)source, patch);
		}

		#endregion

		private Func<HashSet<TItemType>, TType> CompileConvertor()
		{
			if (typeof(TType).IsGenericType
							&& (typeof(TType).GetGenericTypeDefinition() == typeof(HashSet<>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(ISet<>)))
				return x => (TType)(object)x;

			if (typeof(TType).IsArray)
				return x => (TType)(object)x.ToArray();

			ParameterExpression from = Expression.Parameter(typeof(HashSet<TItemType>), "from");

			Expression<Func<HashSet<TItemType>, TType>> convertor = Expression.Lambda<Func<HashSet<TItemType>, TType>>(
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
			HashSet<TItemType> ret = new HashSet<TItemType>(source, this.aItemComparer);

			foreach (IDiffUnorderedCollectionItem item in patch)
			{
				if (item is IDiffItemAdded<TItemType>)
					ret.Add(((IDiffItemAdded<TItemType>) item).NewValue);
				else if (item is IDiffItemRemoved<TItemType>)
					ret.Remove(((IDiffItemRemoved<TItemType>)item).OldValue);
				else if (item is IDiffItemReplaced<TItemType>)
				{
					ret.Remove(((IDiffItemReplaced<TItemType>)item).OldValue);
					ret.Add(((IDiffItemReplaced<TItemType>) item).NewValue);
				}
				else
					throw new InvalidOperationException();
			}

			return this.aConvertor(ret);
		}
	}
}
