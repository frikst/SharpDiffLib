using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionChanged<TItemType> : IDiffItemChanged, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionChanged(int itemIndex, IDiff valueDiff)
		{
			this.ItemIndex = itemIndex;
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
			ret.AppendFormat("{0}={1}:\n", new String('\t', indentLevel), this.ItemIndex);
			ret.Append(this.ValueDiff.ToString(indentLevel + 1));
			return ret.ToString().TrimEnd('\n');
		}

		#endregion

		#region Implementation of IDiffItemChanged

		public IDiff ValueDiff { get; private set; }

		#endregion

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		#endregion
	}
}
