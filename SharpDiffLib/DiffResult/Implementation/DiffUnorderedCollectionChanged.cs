using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffUnorderedCollectionChanged<TIdType, TItemType> : IDiffItemChanged<TItemType>, IDiffUnorderedCollectionItemWithID<TIdType>
	{
		public DiffUnorderedCollectionChanged(TIdType id, IDiff<TItemType> valueDiff)
		{
			this.Id = id;
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

			ret.AppendIndent(indentLevel).Append('=').Append(this.Id).Append(':').AppendLine();
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

		#endregion

		#region Implementation of IDiffItemChanged<TItemType>

		public IDiff<TItemType> ValueDiff { get; private set; }

		public IDiffItemChanged<TItemType> ReplaceWith(IDiff<TItemType> diff)
		{
			return new DiffUnorderedCollectionChanged<TIdType, TItemType>(this.Id, diff);
		}

		#endregion

		#region Implementation of IDiffUnorderedCollectionItemWithID

		public System.Type IdType
			=> typeof(TIdType);

		object IDiffUnorderedCollectionItemWithID.Id
			=> this.Id;

		#endregion

		#region Implementation of IDiffUnorderedCollectionItemWithID<TIdType>

		public TIdType Id { get; private set; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffUnorderedCollectionItemWithID<TIdType> && obj is IDiffItemChanged<TItemType>))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged<TItemType>)obj).ValueDiff)
				&& object.Equals(this.Id, ((IDiffUnorderedCollectionItemWithID<TIdType>)obj).Id);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
