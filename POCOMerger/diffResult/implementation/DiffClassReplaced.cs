using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffClassReplaced<TItemType> : IDiffItemReplaced<TItemType>, IDiffClassItem
	{
		public DiffClassReplaced(Property property, TItemType oldValue, TItemType newValue)
		{
			this.Property = property;
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

			ret.AppendFormat("{0}-{1}:{2}", new String('\t', indentLevel), this.Property.Name, this.OldValue).AppendLine();
			ret.AppendFormat("{0}+{1}:{2}", new String('\t', indentLevel), this.Property.Name, this.NewValue);

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

		#region Implementation of IDiffClassItem

		public Property Property { get; private set; }

		#endregion
	}
}
