using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffUnorderedCollectionChanged<TIdType, TItemType> : IDiffItemChanged, IDiffUnorderedCollectionItemWithID<TIdType>
	{
		public DiffUnorderedCollectionChanged(TIdType id, IDiff valueDiff)
		{
			this.Id = id;
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

		public IDiff ValueDiff { get; private set; }

		#endregion

		#region Implementation of IDiffUnorderedCollectionItemWithID

		public Type IdType
		{
			get { return typeof(TIdType); }
		}

		object IDiffUnorderedCollectionItemWithID.Id
		{
			get { return this.Id; }
		}

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

			if (!(obj is IDiffUnorderedCollectionItemWithID<TIdType> && obj is IDiffItemChanged))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged)obj).ValueDiff)
				&& object.Equals(this.Id, ((IDiffUnorderedCollectionItemWithID<TIdType>)obj).Id);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
