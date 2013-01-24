using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.@base;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.algorithms.applyPatch.collection.unordered
{
	internal class ApplyUnorderedCollectionPatch<TType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Property aIdProperty;
		private IEqualityComparer<TItemType> aItemComparer;

		public ApplyUnorderedCollectionPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIdProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);
		}

		#region Implementation of IApplyPatchAlgorithm<TType>

		public TType Apply(TType source, IDiff<TType> patch)
		{
			if (this.aItemComparer == null)
			{
				if (this.aIdProperty == null)
					this.aItemComparer = EqualityComparer<TItemType>.Default;
				else
					this.aItemComparer = IdHelpers.CreateIdEqualityComparer<TItemType>(this.aIdProperty);
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

			ParameterExpression from = Expression.Parameter(typeof(List<TItemType>), "from");

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
			HashSet<TItemType> sourceSet = new HashSet<TItemType>(source, this.aItemComparer);
			return default(TType);
		}
	}
}
