using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffKeyValueCollectionItemReplaced<TKeyType, TItemType> : IDiffItemReplaced<TItemType>, IDiffKeyValueCollectionItem<TKeyType>
	{
		public DiffKeyValueCollectionItemReplaced(TKeyType key, TItemType oldValue, TItemType newValue)
		{
			this.Key = key;
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

			ret.AppendIndent(indentLevel).Append('-').Append(this.Key).Append(':').AppendNullable(this.OldValue).AppendLine();
			ret.AppendIndent(indentLevel).Append('+').Append(this.Key).Append(':').AppendNullable(this.NewValue);

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
			get { return OldValue; }
		}

		object IDiffItemReplaced.NewValue
		{
			get { return NewValue; }
		}

		#endregion

		#region Implementation of IDiffItemReplaced<TItemType>

		public TItemType NewValue { get; private set; }
		public TItemType OldValue { get; private set; }

		#endregion

		#region Implementation of IDiffKeyValueCollectionItem

		public Type KeyType
		{
			get { return typeof(TKeyType); }
		}

		object IDiffKeyValueCollectionItem.Key
		{
			get { return Key; }
		}

		#endregion

		#region Implementation of IDiffKeyValueCollectionItem<TKeyType>

		public TKeyType Key { get; private set; }

		#endregion
	}
}
