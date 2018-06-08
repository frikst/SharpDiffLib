using System;
using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public class ValueDiffItemFactory<TType> : IDiffItemFactory<TType>
	{
		private readonly List<IDiffItem> aDiffItems;

		internal ValueDiffItemFactory()
		{
			this.aDiffItems = new List<IDiffItem>();
		}

		public ValueDiffItemFactory<TType> Replaced(TType oldValue, TType newValue)
		{
			this.aDiffItems.Add(new DiffValueReplaced<TType>(oldValue, newValue));
			return this;
		}

		public ValueDiffItemFactory<TType> ChangedType<TSubType>(Func<InnerDiffFactory.IParameter<TSubType, TSubType>, IDiffItemFactory<TSubType>> diffFactory)
			where TSubType : TType
		{
			var diff = diffFactory(new InnerDiffFactory.Parameter<TSubType, TSubType>()).MakeDiff();

			return this.ChangedType(diff);
		}

		public ValueDiffItemFactory<TType> ChangedType<TSubType>(IDiff<TSubType> diff)
			where TSubType : TType
		{
			this.aDiffItems.Add(new DiffValueChanged<TType>(typeof(TSubType), (IDiff<TType>) diff));
			return this;
		}

		public ValueDiffItemFactory<TType> Changed(Func<InnerDiffFactory.IParameter<TType, TType>, IDiffItemFactory<TType>> diffFactory)
		{
			var diff = diffFactory(new InnerDiffFactory.Parameter<TType, TType>()).MakeDiff();

			return this.Changed(diff);
		}

		public ValueDiffItemFactory<TType> Changed(IDiff<TType> diff)
		{
			this.aDiffItems.Add(new DiffValueChanged<TType>(typeof(TType), diff));
			return this;
		}

		public ValueDiffItemFactory<TType> Conflicted(Action<ValueDiffItemFactory<TType>> left, Action<ValueDiffItemFactory<TType>> right)
		{
			var conflictsLeft = new ValueDiffItemFactory<TType>();
			left(conflictsLeft);

			var conflictsRight = new ValueDiffItemFactory<TType>();
			right(conflictsRight);

			return this.Conflicted(conflictsLeft, conflictsRight);
		}

		public ValueDiffItemFactory<TType> Conflicted(ValueDiffItemFactory<TType> conflictsLeft, ValueDiffItemFactory<TType> conflictsRight)
		{
			this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

			return this;
		}

		public ValueDiffItemFactory<TType> Unchanged(TType value)
		{
			this.aDiffItems.Add(new DiffValueUnchanged<TType>(value));
			return this;
		}

		public ValueDiffItemFactory<TType> Custom(IDiffItem item)
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
