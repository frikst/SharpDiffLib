using System;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

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
			return string.Format("{0}-{1}\n", new String('\t', indentLevel), this.OldValue);
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
	}
}
