using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffUnorderedCollectionWithIDAdded<TItemType, TIdType> : IDiffItemAdded<TItemType>, IDiffUnorderedCollectionItemWithID<TIdType>
	{
		public DiffUnorderedCollectionWithIDAdded(TIdType id, TItemType newValue)
		{
			this.Id = id;
			this.NewValue = newValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('+').Append(this.Id).Append(':').AppendNullable(this.NewValue);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemAdded

		object IDiffItemAdded.NewValue
		{
			get { return this.NewValue; }
		}

		#endregion

		#region Implementation of IDiffItemAdded<TItemType>

		public TItemType NewValue { get; private set; }

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
