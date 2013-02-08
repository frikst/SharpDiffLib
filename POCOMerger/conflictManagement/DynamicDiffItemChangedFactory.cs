using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.@internal;

namespace POCOMerger.conflictManagement
{
	internal class DynamicDiffItemChangedFactory
	{
		private static readonly Dictionary<Type, Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>> aCreateItemChangedEnvelopeCache
			= new Dictionary<Type, Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>>();

		private static readonly Dictionary<Type, Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>> aCreateFinishedItemChangedEnvelopeCache
			= new Dictionary<Type, Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>>();

		private readonly Dictionary<IDiffItemConflicted, ResolveAction> aResolveActions;
		private bool aFinishItems;
		private readonly Dictionary<Type, Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>> aCreateItemChangedEnvelopeCacheLocal;

		public DynamicDiffItemChangedFactory(Dictionary<IDiffItemConflicted, ResolveAction> resolveActions, bool finishItems)
		{
			this.aResolveActions = resolveActions;

			this.aCreateItemChangedEnvelopeCacheLocal = finishItems ? aCreateItemChangedEnvelopeCache : aCreateFinishedItemChangedEnvelopeCache;
			this.aFinishItems = finishItems;
		}

		public IDiffItem Create(IDiffItemChanged item)
		{
			Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged> envelope;
			if (!aCreateItemChangedEnvelopeCache.TryGetValue(item.ItemType, out envelope))
				envelope = aCreateItemChangedEnvelopeCache[item.ItemType] = this.CompileCreateItemChangedEnvelope(item.ItemType);

			return envelope(item, this.aResolveActions);
		}

		private Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged> CompileCreateItemChangedEnvelope(Type type)
		{
			ParameterExpression originalItem = Expression.Parameter(typeof(IDiffItemChanged), "originalItem");
			ParameterExpression resolveActions = Expression.Parameter(typeof(IDiffItemChanged), "resolveActions");

			Expression newDiff = Expression.New(
				Members.DynamicDiffMembers.New(type),
				Expression.Property(
					Expression.Convert(
						originalItem,
						typeof(IDiffItemChanged<>).MakeGenericType(type)
					),
					Members.DiffItems.ChangedDiff(type)
				),
				resolveActions
			);

			if (this.aFinishItems)
			{
				newDiff = Expression.Call(
					newDiff,
					Members.DynamicDiffMembers.Finish(type)
				);
			}

			var function = Expression.Lambda<Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>>(
				Expression.Call(
					Expression.Convert(
						originalItem,
						typeof(IDiffItemChanged<>).MakeGenericType(type)
					),
					Members.DiffItems.ReplaceDiffWith(type),
					newDiff
				),
				originalItem, resolveActions
			);

			return function.Compile();
		}
	}
}