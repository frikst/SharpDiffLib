using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffUnorderedCollectionRemoved<TItemType> : IDiffItemRemoved<TItemType>, IDiffUnorderedCollectionItem
	{
		public DiffUnorderedCollectionRemoved(TItemType oldValue)
		{
			this.OldValue = oldValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('-').AppendNullable(this.OldValue);

			return ret.ToString();
		}

		#endregion

		#region Implementation of IDiffItemRemoved

		object IDiffItemRemoved.OldValue
		{
			get { return this.OldValue; }
		}

		#endregion

		#region Implementation of IDiffItemRemoved<TItemType>

		public TItemType OldValue { get; private set; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffUnorderedCollectionItem && obj is IDiffItemRemoved<TItemType>))
				return false;

			return object.Equals(this.OldValue, ((IDiffItemRemoved<TItemType>)obj).OldValue);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
