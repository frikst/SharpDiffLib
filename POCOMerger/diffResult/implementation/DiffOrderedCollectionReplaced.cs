using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionReplaced<TItemType> : IDiffItemReplaced<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionReplaced(int itemIndex, TItemType oldValue, TItemType newValue)
		{
			this.ItemIndex = itemIndex;
			this.OldValue = oldValue;
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

			ret.AppendIndent(indentLevel).Append('-').Append(this.ItemIndex).Append(':').AppendNullable(this.OldValue).AppendLine();
			ret.AppendIndent(indentLevel).Append('+').Append(this.ItemIndex).Append(':').AppendNullable(this.NewValue);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemReplaced

		object IDiffItemReplaced.OldValue
		{
			get { return this.OldValue; }
		}

		object IDiffItemReplaced.NewValue
		{
			get { return this.NewValue; }
		}

		#endregion

		#region Implementation of IDiffItemReplaced<TItemType>

		public TItemType NewValue { get; private set; }

		public TItemType OldValue { get; private set; }

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		public IDiffOrderedCollectionItem CreateWithDelta(int delta)
		{
			return new DiffOrderedCollectionReplaced<TItemType>(this.ItemIndex + delta, this.OldValue, this.NewValue);
		}

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffOrderedCollectionItem && obj is IDiffItemReplaced<TItemType>))
				return false;

			return object.Equals(this.OldValue, ((IDiffItemReplaced<TItemType>)obj).OldValue)
				&& object.Equals(this.NewValue, ((IDiffItemReplaced<TItemType>)obj).NewValue)
				&& object.Equals(this.ItemIndex, ((IDiffOrderedCollectionItem)obj).ItemIndex);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
