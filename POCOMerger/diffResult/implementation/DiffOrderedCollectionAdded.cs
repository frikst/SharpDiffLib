using System;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionAdded<TItemType> : IDiffItemAdded<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionAdded(int itemIndex, TItemType newValue)
		{
			this.ItemIndex = itemIndex;
			this.NewValue = newValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			return string.Format("{0}+{1}:{2}\n", new String('\t', indentLevel), this.ItemIndex, this.NewValue);
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

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		#endregion
	}
}
