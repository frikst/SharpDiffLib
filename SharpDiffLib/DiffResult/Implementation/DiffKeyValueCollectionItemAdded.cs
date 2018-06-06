using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffKeyValueCollectionItemAdded<TKeyType, TItemType> : IDiffKeyValueCollectionItem<TKeyType>, IDiffItemAdded<TItemType>
	{
		public DiffKeyValueCollectionItemAdded(TKeyType key, TItemType newValue)
		{
			this.Key = key;
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

			ret.AppendIndent(indentLevel).Append('+').Append(this.Key).Append(':').AppendNullable(this.NewValue);

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

		public TItemType NewValue { get; }

		#endregion

		#region Implementation of IDiffKeyValueCollectionItem

		public System.Type KeyType
			=> typeof(TKeyType);

		object IDiffKeyValueCollectionItem.Key
			=> this.Key;

		#endregion

		#region Implementation of IDiffKeyValueCollectionItem<TKeyType>

		public TKeyType Key { get; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffKeyValueCollectionItem<TKeyType> && obj is IDiffItemAdded<TItemType>))
				return false;

			return object.Equals(this.NewValue, ((IDiffItemAdded<TItemType>)obj).NewValue)
				&& object.Equals(this.Key, ((IDiffKeyValueCollectionItem<TKeyType>)obj).Key);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
