using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffOrderedCollectionAdded<TItemType> : IDiffItemAdded<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionAdded(int itemIndex, TItemType newValue)
		{
			this.ItemIndex = itemIndex;
			this.NewValue = newValue;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemAdded<TItemType>))
				return false;

			return object.Equals(this.NewValue, ((IDiffItemAdded<TItemType>) other).NewValue);
		}

		public System.Type ItemType
			=> typeof(TItemType);

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
			=> this.NewValue;

		#endregion

		#region Implementation of IDiffItemAdded<TItemType>

		public TItemType NewValue { get; private set; }

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
