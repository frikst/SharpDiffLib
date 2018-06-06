using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;
using KST.SharpDiffLib.Internal.Members;

namespace KST.SharpDiffLib.ConflictManagement
{
	internal class DynamicDiffItemChangedFactory
	{
		private static readonly Dictionary<(Type itemType, Type diffType), Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>> aCreateItemChangedEnvelopeCache
			= new Dictionary<(Type itemType, Type diffType), Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>>();

		private readonly Dictionary<IDiffItemConflicted, ResolveAction> aResolveActions;
		private readonly bool aFinishItems;

		public DynamicDiffItemChangedFactory(Dictionary<IDiffItemConflicted, ResolveAction> resolveActions, bool finishItems)
		{
			this.aResolveActions = resolveActions;
			this.aFinishItems = finishItems;
		}

		public IDiffItem Create(IDiffItemChanged item)
		{
			(Type itemType, Type diffType) key;

			if (item is IDiffValue valueItem)
				key = (valueItem.ItemType, valueItem.ValueType);
			else
				key = (item.ItemType, item.ItemType);

			Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged> envelope;
			if (!aCreateItemChangedEnvelopeCache.TryGetValue(key, out envelope))
				envelope = aCreateItemChangedEnvelopeCache[key] = this.CompileCreateItemChangedEnvelope(key.Item1, key.Item2);

			return envelope(item, this.aResolveActions);
		}

		private Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged> CompileCreateItemChangedEnvelope(Type itemType, Type diffType)
		{
			ParameterExpression originalItem = Expression.Parameter(typeof(IDiffItem), "originalItem");
			ParameterExpression resolveActions = Expression.Parameter(typeof(Dictionary<IDiffItemConflicted, ResolveAction>), "resolveActions");

			Expression newDiff = Expression.New(
				DynamicDiffMembers.New(diffType),
				Expression.Convert(
					Expression.Property(
						Expression.Convert(
							originalItem,
							typeof(IDiffItemChanged<>).MakeGenericType(itemType)
						),
						DiffItemsMembers.ChangedDiff(itemType)
					),
					typeof(IDiff<>).MakeGenericType(diffType)
				),
				resolveActions
			);

			if (this.aFinishItems)
			{
				newDiff = Expression.Call(
					newDiff,
					DynamicDiffMembers.Finish(diffType)
				);
			}

			var function = Expression.Lambda<Func<IDiffItem, Dictionary<IDiffItemConflicted, ResolveAction>, IDiffItemChanged>>(
				Expression.Call(
					Expression.Convert(
						originalItem,
						typeof(IDiffItemChanged<>).MakeGenericType(itemType)
					),
					DiffItemsMembers.ReplaceDiffWith(itemType),
					newDiff
				),
				originalItem, resolveActions
			);

			return function.Compile();
		}
	}
}