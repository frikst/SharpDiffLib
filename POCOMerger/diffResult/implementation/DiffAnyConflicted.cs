using System;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffAnyConflicted : IDiffItemConflicted
	{
		public DiffAnyConflicted(IDiffItem left, IDiffItem right)
		{
			this.Left = left;
			this.Right = right;
		}

		#region Implementation of IDiffItem

		public Type ItemType
		{
			get { return null; }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append("C<<<").AppendLine();
			ret.Append(this.Left.ToString(indentLevel + 1)).AppendLine();
			ret.AppendIndent(indentLevel).Append("C>>>").AppendLine();
			ret.Append(this.Right.ToString(indentLevel + 1));

			return ret.ToString();
		}

		#endregion

		#region Implementation of IDiffItemConflicted

		public IDiffItem Left { get; private set; }

		public IDiffItem Right { get; private set; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffItemConflicted))
				return false;

			return object.Equals(this.Left, ((IDiffItemConflicted)obj).Left)
				&& object.Equals(this.Right, ((IDiffItemConflicted)obj).Right);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
