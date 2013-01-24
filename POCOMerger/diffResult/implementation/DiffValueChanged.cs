﻿using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffValueChanged<TItemType> : IDiffItemChanged, IDiffValue
	{
		public DiffValueChanged(IDiff valueDiff)
		{
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

			ret.AppendIndent(indentLevel).Append("=:").AppendLine();
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
	}
}
