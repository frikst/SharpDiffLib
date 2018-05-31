using System.Collections;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.ConflictManagement
{
	internal class DynamicDiffIterator : IEnumerable<IDiffItem>
	{
		private readonly IDiff aOriginal;
		private readonly DynamicDiffItemChangedFactory aDynamicDiffItemChangedFactory;
		private readonly Dictionary<IDiffItemConflicted, ResolveAction> aResolveActions;

		public DynamicDiffIterator(IDiff original, DynamicDiffItemChangedFactory dynamicDiffItemChangedFactory, Dictionary<IDiffItemConflicted, ResolveAction> resolveActions)
		{
			this.aOriginal = original;
			this.aDynamicDiffItemChangedFactory = dynamicDiffItemChangedFactory;
			this.aResolveActions = resolveActions;
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
								foreach (var left in this.PostprocessItemsFromConflict(conflict.Left, conflict))
									yield return left;
								break;
							case ResolveAction.UseRight:
								foreach (var right in this.PostprocessItemsFromConflict(conflict.Right, conflict))
									yield return right;
								break;
							case ResolveAction.LeftThenRight:
								foreach (var left in this.PostprocessItemsFromConflict(conflict.Left, conflict))
									yield return left;
								foreach (var right in this.PostprocessItemsFromConflict(conflict.Right, conflict))
									yield return right;
								break;
							case ResolveAction.RightThenLeft:
								foreach (var right in this.PostprocessItemsFromConflict(conflict.Right, conflict))
									yield return right;
								foreach (var left in this.PostprocessItemsFromConflict(conflict.Left, conflict))
									yield return left;
								break;
						}
					}
					else
						yield return item;
				}
				else if (item is IDiffItemChanged)
					yield return this.aDynamicDiffItemChangedFactory.Create((IDiffItemChanged)item);
				else
					yield return item;
			}
		}

		#endregion

		#region Implementation of IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		private IEnumerable<IDiffItem> PostprocessItemsFromConflict(IEnumerable<IDiffItem> items, IDiffItemConflicted conflict)
		{
			foreach (IDiffItem item in items)
			{
				IDiffItem ret = item;

				yield return ret;
			}
		}
	}
}