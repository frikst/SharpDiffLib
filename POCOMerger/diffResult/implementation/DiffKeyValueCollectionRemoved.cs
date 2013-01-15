using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffKeyValueCollectionRemoved<TKeyType, TItemType> : IDiffItemRemoved<TItemType>, IDiffKeyValueCollection<TKeyType>
	{
		public DiffKeyValueCollectionRemoved(TKeyType key, TItemType oldValue)
		{
			this.Key = key;
			this.OldValue = oldValue;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('-').Append(this.Key).Append(':').AppendNullable(this.OldValue);

			return ret.ToString();
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

		#region Implementation of IDiffKeyValueCollection

		public Type KeyType
		{
			get { return typeof(TKeyType); }
		}

		object IDiffKeyValueCollection.Key
		{
			get { return this.Key; }
		}

		#endregion

		#region Implementation of IDiffKeyValueCollection<TKeyType>

		public TKeyType Key { get; private set; }

		#endregion
	}
}
