﻿using System;
using System.Text;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.type;
using SharpDiffLib.@internal;

namespace SharpDiffLib.diffResult.implementation
{
	internal class DiffOrderedCollectionChanged<TItemType> : IDiffItemChanged<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionChanged(int itemIndex, IDiff<TItemType> valueDiff)
		{
			this.ItemIndex = itemIndex;
			this.ValueDiff = valueDiff;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemChanged<TItemType>))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged<TItemType>) other).ValueDiff);
		}

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('=').Append(this.ItemIndex).Append(':').AppendLine();
			ret.Append(this.ValueDiff.ToString(indentLevel + 1));

			return ret.ToString();
		}

		#endregion

		#region Implementation of IDiffItemChanged

		IDiff IDiffItemChanged.ValueDiff
		{
			get { return this.ValueDiff; }
		}

		#endregion

		#region Implementation of IDiffItemChanged<TItemType>

		public IDiff<TItemType> ValueDiff { get; private set; }

		public IDiffItemChanged<TItemType> ReplaceWith(IDiff<TItemType> diff)
		{
			return new DiffOrderedCollectionChanged<TItemType>(this.ItemIndex, diff);
		}

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		public IDiffOrderedCollectionItem CreateWithDelta(int delta)
		{
			if (delta == 0)
				return this;
			return new DiffOrderedCollectionChanged<TItemType>(this.ItemIndex + delta, this.ValueDiff);
		}

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffOrderedCollectionItem && obj is IDiffItemChanged<TItemType>))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged<TItemType>)obj).ValueDiff)
				&& object.Equals(this.ItemIndex, ((IDiffOrderedCollectionItem)obj).ItemIndex);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
