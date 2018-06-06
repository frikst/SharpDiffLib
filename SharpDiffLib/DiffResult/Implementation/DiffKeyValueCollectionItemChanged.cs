using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffKeyValueCollectionItemChanged<TKeyType, TItemType> : IDiffKeyValueCollectionItem<TKeyType>, IDiffItemChanged<TItemType>
	{
		public DiffKeyValueCollectionItemChanged(TKeyType key, IDiff<TItemType> valueDiff)
		{
			this.Key = key;
			this.ValueDiff = valueDiff;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemChanged<TItemType>))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged<TItemType>) other).ValueDiff);
		}

		public System.Type ItemType
			=> typeof(TItemType);

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('=').Append(this.Key).Append(':').AppendLine();
			ret.Append(this.ValueDiff.ToString(indentLevel + 1));

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemChanged

		IDiff IDiffItemChanged.ValueDiff
			=> this.ValueDiff;

		public IDiffItemChanged<TItemType> ReplaceWith(IDiff<TItemType> diff)
		{
			return new DiffKeyValueCollectionItemChanged<TKeyType, TItemType>(this.Key, diff);
		}

		#endregion

		#region Implementation of IDiffItemChanged<TItemType>

		public IDiff<TItemType> ValueDiff { get; }

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

			if (!(obj is IDiffKeyValueCollectionItem<TKeyType> && obj is IDiffItemChanged<TItemType>))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged<TItemType>)obj).ValueDiff)
				&& object.Equals(this.Key, ((IDiffKeyValueCollectionItem<TKeyType>)obj).Key);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
