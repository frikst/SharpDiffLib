using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffClassChanged<TItemType> : IDiffItemChanged, IDiffClassItem
	{
		public DiffClassChanged(Property property, IDiff diff)
		{
			this.ValueDiff = diff;
			this.Property = property;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('=').Append(this.Property.Name).Append(':').AppendLine();
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

		#region Implementation of IDiffClassItem

		public Property Property { get; private set; }

		#endregion
	}
}
