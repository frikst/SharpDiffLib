using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class Diff<TType> : IDiff<TType>
	{
		private readonly List<IDiffItem> aItems;

		public Diff(List<IDiffItem> items)
		{
			this.aItems = items;
		}

		#region Implementation of IEnumerable<TType>

		public IEnumerator<IDiffItem> GetEnumerator()
		{
			return this.aItems.GetEnumerator();
		}

		#endregion

		#region Implementation of IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Implementation of ICountableEnumerable<IDiffItem>

		public int Count
		{
			get { return this.aItems.Count; }
		}

		#endregion

		#region Implementation of IDiff

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			foreach (IDiffItem diffItem in this)
				ret.AppendLine(diffItem.ToString(indentLevel));

			return ret.ToString().TrimEnd('\r', '\n');
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		public bool HasChanges
		{
			get { return this.aItems.Count > 0 && !this.aItems.All(x => x is IDiffItemUnchanged); }
		}

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiff<TType>))
				return false;

			return this.aItems.SequenceEqual((IDiff<TType>) obj);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
