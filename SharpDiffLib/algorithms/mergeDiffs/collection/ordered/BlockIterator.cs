using System.Collections.Generic;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;

namespace POCOMerger.algorithms.mergeDiffs.collection.ordered
{
	internal class BlockIterator
	{
		private IDiffOrderedCollectionItem aNextItem;
		private IEnumerator<IDiffItem> aEnumerator;
		private int aIndexDelta;

		public BlockIterator(IEnumerable<IDiffItem> items)
		{
			this.aEnumerator = items.GetEnumerator();
			this.aIndexDelta = 0;
			this.aNextItem = null;

			this.BlockIndex = 0;
			this.Items = new List<IDiffOrderedCollectionItem>();
			this.IsEnd = false;

			this.Next();
		}

		public void Next()
		{
			this.Items.Clear();

			if (this.aNextItem == null && !this.aEnumerator.MoveNext())
			{
				this.BlockIndex = -1;
				this.IsEnd = true;
				return;
			}

			IDiffOrderedCollectionItem item;
			if (this.aNextItem == null)
				item = this.GetCurrentItem();
			else
				item = this.aNextItem;

			this.aNextItem = null;

			this.Items.Add(item);
			this.BlockIndex = item.ItemIndex;

			if (item is IDiffItemRemoved)
			{
				this.RetrieveWholeBlock<IDiffItemRemoved>();
				this.aIndexDelta += this.Items.Count;
			}
			else if (item is IDiffItemAdded)
			{
				this.RetrieveWholeBlock<IDiffItemAdded>();
			}
		}

		private void RetrieveWholeBlock<TItemAction>()
		{
			while (this.aEnumerator.MoveNext())
			{
				IDiffOrderedCollectionItem item = this.GetCurrentItem();

				if (item is TItemAction && item.ItemIndex == this.BlockIndex)
					this.Items.Add(item);
				else
				{
					this.aNextItem = item;
					break;
				}
			}
		}

		private IDiffOrderedCollectionItem GetCurrentItem()
		{
			return ((IDiffOrderedCollectionItem) this.aEnumerator.Current).CreateWithDelta(this.aIndexDelta);
		}

		public int BlockIndex { get; set; }
		public List<IDiffOrderedCollectionItem> Items { get; private set; }
		public bool IsEnd { get; private set; }
	}
}
