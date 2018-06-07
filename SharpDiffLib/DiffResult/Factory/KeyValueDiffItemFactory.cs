using System;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public class KeyValueDiffItemFactory<TType, TKey, TValue>
	{
		private readonly List<IDiffItem> aDiffItems;
	
		internal KeyValueDiffItemFactory()
		{
			this.aDiffItems = new List<IDiffItem>();
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Added(TKey key, TValue newValue)
		{
			this.aDiffItems.Add(new DiffKeyValueCollectionItemAdded<TKey, TValue>(key, newValue));
			return this;
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Removed(TKey key, TValue oldValue)
		{
			this.aDiffItems.Add(new DiffKeyValueCollectionItemRemoved<TKey, TValue>(key, oldValue));
			return this;
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Replaced(TKey key, TValue oldValue, TValue newValue)
		{
			this.aDiffItems.Add(new DiffKeyValueCollectionItemReplaced<TKey, TValue>(key, oldValue, newValue));
			return this;
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Changed(TKey key, IDiff<TValue> diff)
		{
			this.aDiffItems.Add(new DiffKeyValueCollectionItemChanged<TKey, TValue>(key, diff));
			return this;
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Conflicted(Action<KeyValueDiffItemFactory<TType, TKey, TValue>> left, Action<KeyValueDiffItemFactory<TType, TKey, TValue>> right)
		{
			var conflictsLeft = new KeyValueDiffItemFactory<TType, TKey, TValue>();
			left(conflictsLeft);

			var conflictsRight = new KeyValueDiffItemFactory<TType, TKey, TValue>();
			right(conflictsRight);

			this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

			return this;
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Unchanged(TKey key, TValue value)
		{
			this.aDiffItems.Add(new DiffKeyValueCollectionItemUnchanged<TKey, TValue>(key, value));
			return this;
		}

		public KeyValueDiffItemFactory<TType, TKey, TValue> Custom(IDiffItem item)
		{
			this.aDiffItems.Add(item);
			return this;
		}

		public IDiff<TType> MakeDiff()
		{
			return new Diff<TType>(this.aDiffItems);
		}
	}
}
