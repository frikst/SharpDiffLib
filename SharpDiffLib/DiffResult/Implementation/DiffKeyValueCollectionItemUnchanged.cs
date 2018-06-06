using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffKeyValueCollectionItemUnchanged<TKeyType, TItemType> : IDiffItemUnchanged<TItemType>, IDiffKeyValueCollectionItem<TKeyType>
	{
		public DiffKeyValueCollectionItemUnchanged(TKeyType key, TItemType value)
		{
			this.Key = key;
			this.Value = value;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemUnchanged<TItemType>))
				return false;

			return object.Equals(this.Value, ((IDiffItemUnchanged<TItemType>)other).Value);
		}

		public System.Type ItemType
			=> typeof(TItemType);

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append(' ').Append(this.Key).Append(':').AppendNullable(this.Value);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemUnchanged

		object IDiffItemUnchanged.Value
			=> this.Value;

		#endregion

		#region Implementation of IDiffItemUnchanged<TItemType>

		public TItemType Value { get; }

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

			if (!(obj is IDiffKeyValueCollectionItem<TKeyType> && obj is IDiffItemUnchanged<TItemType>))
				return false;

			return object.Equals(this.Value, ((IDiffItemUnchanged<TItemType>)obj).Value)
				&& object.Equals(this.Key, ((IDiffKeyValueCollectionItem<TKeyType>)obj).Key);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
