using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.type;
using SharpDiffLib.@internal;

namespace SharpDiffLib.diffResult.implementation
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

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

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
		{
			get { return this.Value; }
		}

		#endregion

		#region Implementation of IDiffItemUnchanged<TItemType>

		public TItemType Value { get; private set; }

		#endregion

		#region Implementation of IDiffKeyValueCollectionItem

		public Type KeyType
		{
			get { return typeof(TKeyType); }
		}

		object IDiffKeyValueCollectionItem.Key
		{
			get { return this.Key; }
		}

		#endregion

		#region Implementation of IDiffKeyValueCollectionItem<TKeyType>

		public TKeyType Key { get; private set; }

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
