using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionAdded<TItemType> : IDiffItemAdded<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionAdded(int itemIndex, TItemType newValue)
		{
			this.ItemIndex = itemIndex;
			this.NewValue = newValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('+').Append(this.ItemIndex).Append(':').AppendNullable(this.NewValue);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemAdded

		object IDiffItemAdded.NewValue
		{
			get { return this.NewValue; }
		}

		#endregion

		#region Implementation of IDiffItemAdded<TItemType>

		public TItemType NewValue { get; private set; }

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		public IDiffOrderedCollectionItem CreateWithDelta(int delta)
		{
			if (delta == 0)
				return this;
			return new DiffOrderedCollectionAdded<TItemType>(this.ItemIndex + delta, this.NewValue);
		}

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffOrderedCollectionItem && obj is IDiffItemAdded<TItemType>))
				return false;

			return object.Equals(this.NewValue, ((IDiffItemAdded<TItemType>)obj).NewValue)
				&& object.Equals(this.ItemIndex, ((IDiffOrderedCollectionItem)obj).ItemIndex);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
