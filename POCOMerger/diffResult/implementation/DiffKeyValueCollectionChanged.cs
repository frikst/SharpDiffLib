using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffKeyValueCollectionChanged<TKeyType, TItemType> : IDiffKeyValueCollection<TKeyType>, IDiffItemChanged
	{
		public DiffKeyValueCollectionChanged(TKeyType key, IDiff valueDiff)
		{
			this.Key = key;
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

			ret.AppendIndent(indentLevel).Append('=').Append(this.Key).Append(':');
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
