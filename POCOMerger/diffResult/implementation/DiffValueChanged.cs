using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffValueChanged<TItemType> : IDiffItemChanged<TItemType>, IDiffValue
	{
		public DiffValueChanged(IDiff<TItemType> valueDiff)
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

		IDiff IDiffItemChanged.ValueDiff
		{
			get { return this.ValueDiff; }
		}

		#endregion

		#region Implementation of IDiffItemChanged<TItemType>

		public IDiff<TItemType> ValueDiff { get; private set; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffValue && obj is IDiffItemChanged<TItemType>))
				return false;

			return object.Equals(this.ValueDiff, ((IDiffItemChanged<TItemType>)obj).ValueDiff);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
