using System;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public class OrderedDiffItemFactory<TType, TValue> : IDiffItemFactory<TType>
	{
		private readonly List<IDiffItem> aDiffItems;
	
		internal OrderedDiffItemFactory()
		{
			this.aDiffItems = new List<IDiffItem>();
		}

		public OrderedDiffItemFactory<TType, TValue> Added(int index, TValue newValue)
		{
			this.aDiffItems.Add(new DiffOrderedCollectionAdded<TValue>(index, newValue));
			return this;
		}

		public OrderedDiffItemFactory<TType, TValue> Removed(int index, TValue oldValue)
		{
			this.aDiffItems.Add(new DiffOrderedCollectionRemoved<TValue>(index, oldValue));
			return this;
		}

		public OrderedDiffItemFactory<TType, TValue> Replaced(int index, TValue oldValue, TValue newValue)
		{
			this.aDiffItems.Add(new DiffOrderedCollectionReplaced<TValue>(index, oldValue, newValue));
			return this;
		}

		public OrderedDiffItemFactory<TType, TValue> Changed(int index, Func<InnerDiffFactory.IParameter<TValue, TValue>, IDiffItemFactory<TValue>> diffFactory)
		{
			var diff = diffFactory(new InnerDiffFactory.Parameter<TValue, TValue>()).MakeDiff();

			return this.Changed(index, diff);
		}

		public OrderedDiffItemFactory<TType, TValue> Changed(int index, IDiff<TValue> diff)
		{
			this.aDiffItems.Add(new DiffOrderedCollectionChanged<TValue>(index, diff));
			return this;
		}

		public OrderedDiffItemFactory<TType, TValue> Conflicted(Action<OrderedDiffItemFactory<TType, TValue>> left, Action<OrderedDiffItemFactory<TType, TValue>> right)
		{
			var conflictsLeft = new OrderedDiffItemFactory<TType, TValue>();
			left(conflictsLeft);

			var conflictsRight = new OrderedDiffItemFactory<TType, TValue>();
			right(conflictsRight);

			return this.Conflicted(conflictsLeft, conflictsRight);
		}

		public OrderedDiffItemFactory<TType, TValue> Conflicted(OrderedDiffItemFactory<TType, TValue> conflictsLeft, OrderedDiffItemFactory<TType, TValue> conflictsRight)
		{
			this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

			return this;
		}

		public OrderedDiffItemFactory<TType, TValue> Unchanged(int index, TValue value)
		{
			this.aDiffItems.Add(new DiffOrderedCollectionUnchanged<TValue>(index, value));
			return this;
		}

		public OrderedDiffItemFactory<TType, TValue> Custom(IDiffItem item)
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
