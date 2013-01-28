﻿using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionChanged<TItemType> : IDiffItemChanged, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionChanged(int itemIndex, IDiff valueDiff)
		{
			this.ItemIndex = itemIndex;
			this.ValueDiff = valueDiff;
		}

		#region Implementation of IDiffItem

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

		public IDiff ValueDiff { get; private set; }

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		public IDiffOrderedCollectionItem CreateWithDelta(int delta)
		{
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

			if (!(obj is IDiffOrderedCollectionItem && obj is IDiffItemChanged))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged)obj).ValueDiff)
				&& object.Equals(this.ItemIndex, ((IDiffOrderedCollectionItem)obj).ItemIndex);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
