using System;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffUnorderedCollectionAdded<TItemType> : IDiffItemAdded<TItemType>, IDiffUnorderedCollectionItem
	{
		public DiffUnorderedCollectionAdded(TItemType newValue)
		{
			this.NewValue = newValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			return string.Format("{0}+{1}\n", new String('\t', indentLevel), this.NewValue);
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
	}
}
