using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffOrderedCollectionRemoved<TItemType> : IDiffItemRemoved<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionRemoved(int itemIndex, TItemType oldValue)
		{
			this.ItemIndex = itemIndex;
			this.OldValue = oldValue;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemRemoved<TItemType>))
				return false;

			return object.Equals(this.OldValue, ((IDiffItemRemoved<TItemType>) other).OldValue);
		}

		public System.Type ItemType
			=> typeof(TItemType);

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('-').Append(this.ItemIndex).Append(':').AppendNullable(this.OldValue);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemRemoved

		object IDiffItemRemoved.OldValue
			=> this.OldValue;

		#endregion

		#region Implementation of IDiffItemRemoved<TItemType>

		public TItemType OldValue { get; private set; }

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffOrderedCollectionItem && obj is IDiffItemRemoved<TItemType>))
				return false;

			return object.Equals(this.OldValue, ((IDiffItemRemoved<TItemType>)obj).OldValue)
				&& object.Equals(this.ItemIndex, ((IDiffOrderedCollectionItem)obj).ItemIndex);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
