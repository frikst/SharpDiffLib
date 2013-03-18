using System.Collections;
using System.Collections.Generic;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;

namespace POCOMerger.conflictManagement
{
	internal class DynamicDiffIterator : IEnumerable<IDiffItem>
	{
		private readonly IDiff aOriginal;
		private readonly DynamicDiffItemChangedFactory aDynamicDiffItemChangedFactory;
		private readonly Dictionary<IDiffItemConflicted, ResolveAction> aResolveActions;
		private int aDelta;

		public DynamicDiffIterator(IDiff original, DynamicDiffItemChangedFactory dynamicDiffItemChangedFactory, Dictionary<IDiffItemConflicted, ResolveAction> resolveActions)
		{
			this.aOriginal = original;
			this.aDynamicDiffItemChangedFactory = dynamicDiffItemChangedFactory;
			this.aResolveActions = resolveActions;
			this.aDelta = 0;
		}

		#region Implementation of IEnumerable<IDiffItem>

		public IEnumerator<IDiffItem> GetEnumerator()
		{
			foreach (IDiffItem item in this.aOriginal)
			{
				if (item is IDiffItemConflicted)
				{
					IDiffItemConflicted conflict = (IDiffItemConflicted)item;

					ResolveAction action;
					if (this.aResolveActions.TryGetValue(conflict, out action))
					{
						switch (action)
						{
							case ResolveAction.UseLeft:
								foreach (var left in this.PostprocessItemsFromConflict(conflict.Left))
									yield return left;
								break;
							case ResolveAction.UseRight:
								foreach (var right in this.PostprocessItemsFromConflict(conflict.Right))
									yield return right;
								break;
							case ResolveAction.LeftThenRight:
								foreach (var left in this.PostprocessItemsFromConflict(conflict.Left))
									yield return left;
								foreach (var right in this.PostprocessItemsFromConflict(conflict.Right))
									yield return right;
								break;
							case ResolveAction.RightThenLeft:
								foreach (var right in this.PostprocessItemsFromConflict(conflict.Right))
									yield return right;
								foreach (var left in this.PostprocessItemsFromConflict(conflict.Left))
									yield return left;
								break;
						}
					}
					else
						yield return item;
				}
				else if (item is IDiffItemChanged)
					yield return this.PostprocessItem(this.aDynamicDiffItemChangedFactory.Create((IDiffItemChanged)item));
				else
					yield return this.PostprocessItem(item);
			}
		}

		#endregion

		#region Implementation of IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		private IDiffItem PostprocessItem(IDiffItem item)
		{
			IDiffOrderedCollectionItem collectionItem = item as IDiffOrderedCollectionItem;

			if (collectionItem != null)
				return collectionItem.CreateWithDelta(this.aDelta);
			else
				return item;
		}

		private IEnumerable<IDiffItem> PostprocessItemsFromConflict(IEnumerable<IDiffItem> items)
		{
			int delta = 0;
			foreach (IDiffItem item in items)
			{
				IDiffItem ret = this.PostprocessItem(item);

				if (item is IDiffItemRemoved)
					delta++;

				yield return ret;
			}

			if (delta == 0)
				this.aDelta++;
		}
	}
}