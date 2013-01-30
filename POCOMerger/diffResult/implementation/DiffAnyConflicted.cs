using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.@internal;

namespace POCOMerger.diffResult.implementation
{
	internal class DiffAnyConflicted : IDiffItemConflicted
	{
		private class DiffItemList : ICountableEnumerable<IDiffItem>
		{
			private readonly List<IDiffItem> aItems;

			public DiffItemList(List<IDiffItem> items)
			{
				this.aItems = items;
			}

			public DiffItemList(IDiffItem item)
			{
				this.aItems = new List<IDiffItem> { item };
			}

			#region Implementation of IEnumerable<IDiffItem>

			public IEnumerator<IDiffItem> GetEnumerator()
			{
				return this.aItems.GetEnumerator();
			}

			#endregion

			#region Implementation of IEnumerable

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.aItems.GetEnumerator();
			}

			#endregion

			#region Implementation of ICountableEnumerable<IDiffItem>

			public int Count
			{
				get { return this.aItems.Count; }
			}

			#endregion
		}

		public DiffAnyConflicted(List<IDiffItem> left, List<IDiffItem> right)
		{
			this.Left = new DiffItemList(left);
			this.Right = new DiffItemList(right);
		}

		public DiffAnyConflicted(IDiffItem left, IDiffItem right)
		{
			this.Left = new DiffItemList(left);
			this.Right = new DiffItemList(right);
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemConflicted))
				return false;

			return this.Left.SequenceEqual(((IDiffItemConflicted) other).Left)
				&& this.Right.SequenceEqual(((IDiffItemConflicted) other).Right);
		}

		public Type ItemType
		{
			get { return null; }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append("C<<<").AppendLine();
			foreach (IDiffItem diffItem in Left)
			{
				ret.Append(diffItem.ToString(indentLevel + 1)).AppendLine();
			}
			
			ret.AppendIndent(indentLevel).Append("C>>>").AppendLine();
			foreach (IDiffItem diffItem in Right)
			{
				ret.Append(diffItem.ToString(indentLevel + 1)).AppendLine();
			}

			return ret.ToString().TrimEnd('\r', '\n');
		}

		#endregion

		#region Implementation of IDiffItemConflicted

		public ICountableEnumerable<IDiffItem> Left { get; private set; }

		public ICountableEnumerable<IDiffItem> Right { get; private set; }

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

			return this.Left.SequenceEqual(((IDiffItemConflicted)obj).Left)
				&& this.Right.SequenceEqual(((IDiffItemConflicted)obj).Right);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
