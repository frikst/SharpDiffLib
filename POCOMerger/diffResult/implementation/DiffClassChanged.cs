using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffClassChanged : IDiffItemChanged, IDiffClassItem
	{
		public DiffClassChanged(Property property, IDiff diff)
		{
			this.ValueDiff = diff;
			this.Property = property;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return this.Property.Type; }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();
			ret.AppendFormat("{0}={1}:\n", new String('\t', indentLevel), this.Property.Name);
			ret.Append(this.ValueDiff.ToString(indentLevel + 1));

			//foreach (IDiffItem diffItem in this.ValueDiff)
			//{
			//    ret.AppendLine(diffItem.ToString(indentLevel + 1));
			//}

			return ret.ToString().TrimEnd('\n');
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
