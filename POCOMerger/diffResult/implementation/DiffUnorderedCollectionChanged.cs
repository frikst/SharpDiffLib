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
	}
}
