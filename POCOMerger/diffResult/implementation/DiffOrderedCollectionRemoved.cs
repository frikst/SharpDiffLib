﻿using System;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffOrderedCollectionRemoved<TItemType> : IDiffItemRemoved<TItemType>, IDiffOrderedCollectionItem
	{
		public DiffOrderedCollectionRemoved(int itemIndex, TItemType oldValue)
		{
			this.ItemIndex = itemIndex;
			this.OldValue = oldValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			return string.Format("{0}-{1}:{2}\n", new String('\t', indentLevel), this.ItemIndex, this.OldValue);
		}

		public override string ToString()
		{
			return this.ToString(0);
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

		#region Implementation of IDiffOrderedCollectionItem

		public int ItemIndex { get; private set; }

		#endregion
	}
}
