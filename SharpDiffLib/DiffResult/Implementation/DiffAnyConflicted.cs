using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffAnyConflicted : IDiffItemConflicted
	{
		private class DiffItemList : ICountableEnumerable<IDiffItem>
		{
			private readonly List<IDiffItem> aItems;

			public DiffItemList(IEnumerable<IDiffItem> items)
			{
				this.aItems = items.ToList();
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
				=> this.aItems.Count;

			#endregion
		}

		public DiffAnyConflicted(IEnumerable<IDiffItem> left, IEnumerable<IDiffItem> right)
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

		public System.Type ItemType
			=> null;

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append("C<<<").AppendLine();
			foreach (IDiffItem diffItem in this.Left)
			{
				ret.Append(diffItem.ToString(indentLevel + 1)).AppendLine();
			}
			
			ret.AppendIndent(indentLevel).Append("C>>>").AppendLine();
			foreach (IDiffItem diffItem in this.Right)
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
