using System;
using System.Collections;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Type;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Ordered
{
	internal class MergeJoin : IEnumerable<(List<IDiffOrderedCollectionItem> firstItems, List<IDiffOrderedCollectionItem> secondItems)>
	{
		private readonly IEnumerable<List<IDiffOrderedCollectionItem>> aFirst;
		private readonly IEnumerable<List<IDiffOrderedCollectionItem>> aSecond;

		public MergeJoin(IEnumerable<List<IDiffOrderedCollectionItem>> first, IEnumerable<List<IDiffOrderedCollectionItem>> second)
		{
			this.aFirst = first;
			this.aSecond = second;
		}

		#region Implementation of IEnumerable

		public IEnumerator<(List<IDiffOrderedCollectionItem> firstItems, List<IDiffOrderedCollectionItem> secondItems)> GetEnumerator()
		{
			using (IEnumerator<List<IDiffOrderedCollectionItem>> firstEnumerator = this.aFirst.GetEnumerator())
			{
				using (IEnumerator<List<IDiffOrderedCollectionItem>> secondEnumerator = this.aSecond.GetEnumerator())
				{

					bool firstHasItem = firstEnumerator.MoveNext();
					bool secondHasItem = secondEnumerator.MoveNext();

					while (firstHasItem && secondHasItem)
					{
						if (firstEnumerator.Current[0].ItemIndex < secondEnumerator.Current[0].ItemIndex)
						{
							yield return (firstEnumerator.Current, null);
							firstHasItem = firstEnumerator.MoveNext();
						}
						else if (firstEnumerator.Current[0].ItemIndex > secondEnumerator.Current[0].ItemIndex)
						{
							yield return (null, secondEnumerator.Current);
							secondHasItem = secondEnumerator.MoveNext();
						}
						else
						{
							yield return (firstEnumerator.Current, secondEnumerator.Current);
							firstHasItem = firstEnumerator.MoveNext();
							secondHasItem = secondEnumerator.MoveNext();
						}
					}

					while (firstHasItem)
					{
						yield return (firstEnumerator.Current, null);
						firstHasItem = firstEnumerator.MoveNext();
					}

					while (secondHasItem)
					{
						yield return (null, secondEnumerator.Current);
						secondHasItem = secondEnumerator.MoveNext();
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}