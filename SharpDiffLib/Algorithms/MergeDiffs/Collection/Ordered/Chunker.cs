using System.Collections;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Type;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Ordered
{
	internal class Chunker : IEnumerable<List<IDiffOrderedCollectionItem>>, IEnumerator<List<IDiffOrderedCollectionItem>>
	{
		private readonly IEnumerator<IDiffOrderedCollectionItem> aDiffItemsEnumerator;
		private List<IDiffOrderedCollectionItem> aCurrent;
		private int aIndex;
		private bool aHasCurrent;

		public Chunker(IEnumerable<IDiffOrderedCollectionItem> diffItems)
		{
			this.aDiffItemsEnumerator = diffItems.GetEnumerator();
			this.aHasCurrent = this.aDiffItemsEnumerator.MoveNext();
			this.aCurrent = new List<IDiffOrderedCollectionItem>();
			this.aIndex = int.MinValue;
		}

		#region Implementation of IEnumerable

		public IEnumerator<List<IDiffOrderedCollectionItem>> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			this.aDiffItemsEnumerator.Dispose();
		}

		#endregion

		#region Implementation of IEnumerator

		public bool MoveNext()
		{
			if (!this.aHasCurrent)
				return false;

			this.aCurrent = new List<IDiffOrderedCollectionItem>
			{
				this.aDiffItemsEnumerator.Current
			};

			this.aIndex = this.aDiffItemsEnumerator.Current.ItemIndex;

			while (this.aDiffItemsEnumerator.MoveNext())
			{
				if (this.aIndex != this.aDiffItemsEnumerator.Current.ItemIndex)
					return true;
				this.aCurrent.Add(this.aDiffItemsEnumerator.Current);
			}

			this.aHasCurrent = false;
			return true;
		}

		public void Reset()
		{
			this.aCurrent = new List<IDiffOrderedCollectionItem>();
			this.aDiffItemsEnumerator.Reset();
		}

		public List<IDiffOrderedCollectionItem> Current
			=> this.aCurrent;

		object IEnumerator.Current
			=> this.aCurrent;

		#endregion
	}
}