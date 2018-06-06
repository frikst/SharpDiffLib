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
				switch (item)
				{
					case IDiffItemConflicted itemConflicted:
						ResolveAction action;
						if (this.aResolveActions.TryGetValue(itemConflicted, out action))
						{
							switch (action)
							{
								case ResolveAction.UseLeft:
									foreach (var left in this.PostprocessItemsFromConflict(itemConflicted.Left, itemConflicted))
										yield return left;
									break;
								case ResolveAction.UseRight:
									foreach (var right in this.PostprocessItemsFromConflict(itemConflicted.Right, itemConflicted))
										yield return right;
									break;
								case ResolveAction.LeftThenRight:
									foreach (var left in this.PostprocessItemsFromConflict(itemConflicted.Left, itemConflicted))
										yield return left;
									foreach (var right in this.PostprocessItemsFromConflict(itemConflicted.Right, itemConflicted))
										yield return right;
									break;
								case ResolveAction.RightThenLeft:
									foreach (var right in this.PostprocessItemsFromConflict(itemConflicted.Right, itemConflicted))
										yield return right;
									foreach (var left in this.PostprocessItemsFromConflict(itemConflicted.Left, itemConflicted))
										yield return left;
									break;
							}
						}
						else
							yield return item;

						break;
					case IDiffItemChanged itemChanged:
						yield return this.aDynamicDiffItemChangedFactory.Create(itemChanged);
						break;
					default:
						yield return item;
						break;
				}
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