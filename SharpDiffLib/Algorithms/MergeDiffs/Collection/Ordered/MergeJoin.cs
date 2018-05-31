using System;
using System.Collections;
using System.Collections.Generic;
using SharpDiffLib.diffResult.type;

namespace SharpDiffLib.algorithms.mergeDiffs.collection.ordered
{
	internal class MergeJoin : IEnumerable<Tuple<List<IDiffOrderedCollectionItem>, List<IDiffOrderedCollectionItem>>>
	{
		private readonly IEnumerable<List<IDiffOrderedCollectionItem>> aFirst;
		private readonly IEnumerable<List<IDiffOrderedCollectionItem>> aSecond;

		public MergeJoin(IEnumerable<List<IDiffOrderedCollectionItem>> first, IEnumerable<List<IDiffOrderedCollectionItem>> second)
		{
			this.aFirst = first;
			this.aSecond = second;
		}

		#region Implementation of IEnumerable

		public IEnumerator<Tuple<List<IDiffOrderedCollectionItem>, List<IDiffOrderedCollectionItem>>> GetEnumerator()
		{
			IEnumerator<List<IDiffOrderedCollectionItem>> firstEnumerator = this.aFirst.GetEnumerator();
			IEnumerator<List<IDiffOrderedCollectionItem>> secondEnumerator = this.aSecond.GetEnumerator();

			bool firstHasItem = firstEnumerator.MoveNext();
			bool secondHasItem = secondEnumerator.MoveNext();

			while (firstHasItem && secondHasItem)
			{
				if (firstEnumerator.Current[0].ItemIndex < secondEnumerator.Current[0].ItemIndex)
				{
					yield return Tuple.Create(firstEnumerator.Current, (List<IDiffOrderedCollectionItem>)null);
					firstHasItem = firstEnumerator.MoveNext();
				}
				else if (firstEnumerator.Current[0].ItemIndex > secondEnumerator.Current[0].ItemIndex)
				{
					yield return Tuple.Create((List<IDiffOrderedCollectionItem>)null, secondEnumerator.Current);
					secondHasItem = secondEnumerator.MoveNext();
				}
				else
				{
					yield return Tuple.Create(firstEnumerator.Current, secondEnumerator.Current);
					firstHasItem = firstEnumerator.MoveNext();
					secondHasItem = secondEnumerator.MoveNext();
				}
			}

			while (firstHasItem)
			{
				yield return Tuple.Create(firstEnumerator.Current, (List<IDiffOrderedCollectionItem>) null);
				firstHasItem = firstEnumerator.MoveNext();
			}

			while (secondHasItem)
			{
				yield return Tuple.Create((List<IDiffOrderedCollectionItem>)null, secondEnumerator.Current);
				secondHasItem = secondEnumerator.MoveNext();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}