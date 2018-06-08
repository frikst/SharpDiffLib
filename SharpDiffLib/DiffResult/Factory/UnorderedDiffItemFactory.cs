using System;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public class UnorderedDiffItemFactory<TType, TValue> : IDiffItemFactory<TType>
	{
		private readonly List<IDiffItem> aDiffItems;
	
		internal UnorderedDiffItemFactory()
		{
			this.aDiffItems = new List<IDiffItem>();
		}

		public UnorderedDiffItemFactory<TType, TValue> Added(TValue newValue)
		{
			this.aDiffItems.Add(new DiffUnorderedCollectionAdded<TValue>(newValue));
			return this;
		}

		public UnorderedDiffItemFactory<TType, TValue> Removed(TValue oldValue)
		{
			this.aDiffItems.Add(new DiffUnorderedCollectionRemoved<TValue>(oldValue));
			return this;
		}

		public UnorderedDiffItemFactory<TType, TValue> Replaced(TValue oldValue, TValue newValue)
		{
			this.aDiffItems.Add(new DiffUnorderedCollectionReplaced<TValue>(oldValue, newValue));
			return this;
		}

		public UnorderedDiffItemFactory<TType, TValue> Changed<TId>(TId id, Func<InnerDiffFactory.IDiffFactory<TValue, TValue>, IDiffItemFactory<TValue>> diffFactory)
		{
			var diff = diffFactory(null).MakeDiff();

			return this.Changed(id, diff);
		}

		public UnorderedDiffItemFactory<TType, TValue> Changed<TId>(TId id, IDiff<TValue> diff)
		{
			this.aDiffItems.Add(new DiffUnorderedCollectionChanged<TId, TValue>(id, diff));
			return this;
		}

		public UnorderedDiffItemFactory<TType, TValue> Conflicted(Action<UnorderedDiffItemFactory<TType, TValue>> left, Action<UnorderedDiffItemFactory<TType, TValue>> right)
		{
			var conflictsLeft = new UnorderedDiffItemFactory<TType, TValue>();
			left(conflictsLeft);

			var conflictsRight = new UnorderedDiffItemFactory<TType, TValue>();
			right(conflictsRight);

			return this.Conflicted(conflictsLeft, conflictsRight);
		}

		public UnorderedDiffItemFactory<TType, TValue> Conflicted(UnorderedDiffItemFactory<TType, TValue> conflictsLeft, UnorderedDiffItemFactory<TType, TValue> conflictsRight)
		{
			this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

			return this;
		}

		public UnorderedDiffItemFactory<TType, TValue> Unchanged(TValue value)
		{
			this.aDiffItems.Add(new DiffUnorderedCollectionUnchanged<TValue>(value));
			return this;
		}

		public UnorderedDiffItemFactory<TType, TValue> Custom(IDiffItem item)
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
