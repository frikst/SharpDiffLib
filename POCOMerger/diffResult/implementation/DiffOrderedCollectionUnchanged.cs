using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionUnchanged<TItemType> : IDiffItemUnchanged<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionUnchanged(int itemIndex, TItemType value)
		{
			this.ItemIndex = itemIndex;
			this.Value = value;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemUnchanged<TItemType>))
				return false;

			return object.Equals(this.Value, ((IDiffItemUnchanged<TItemType>)other).Value);
		}

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append(' ').Append(this.ItemIndex).Append(':').AppendNullable(this.Value);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemUnchanged

		object IDiffItemUnchanged.Value
		{
			get { return this.Value; }
		}

		#endregion

		#region Implementation of IDiffItemUnchanged<TItemType>

		public TItemType Value { get; private set; }

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		public IDiffOrderedCollectionItem CreateWithDelta(int delta)
		{
			if (delta == 0)
				return this;
			return new DiffOrderedCollectionUnchanged<TItemType>(this.ItemIndex + delta, this.Value);
		}

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffOrderedCollectionItem && obj is IDiffItemUnchanged<TItemType>))
				return false;

			return object.Equals(this.Value, ((IDiffItemUnchanged<TItemType>)obj).Value)
				&& object.Equals(this.ItemIndex, ((IDiffOrderedCollectionItem)obj).ItemIndex);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
