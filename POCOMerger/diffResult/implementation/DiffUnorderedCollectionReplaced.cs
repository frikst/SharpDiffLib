﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffUnorderedCollectionReplaced<TItemType> : IDiffItemReplaced<TItemType>, IDiffUnorderedCollectionItem
	{
		public DiffUnorderedCollectionReplaced(TItemType oldValue, TItemType newValue)
		{
			this.OldValue = oldValue;
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

			ret.AppendIndent(indentLevel).Append('-').AppendNullable(this.OldValue).AppendLine();
			ret.AppendIndent(indentLevel).Append('+').AppendNullable(this.NewValue);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemReplaced

		object IDiffItemReplaced.OldValue
		{
			get { return this.OldValue; }
		}

		object IDiffItemReplaced.NewValue
		{
			get { return this.NewValue; }
		}

		#endregion

		#region Implementation of IDiffItemReplaced<TItemType>

		public TItemType NewValue { get; private set; }
		public TItemType OldValue { get; private set; }

		#endregion
	}
}