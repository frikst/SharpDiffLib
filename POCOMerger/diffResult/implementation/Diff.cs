using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.implementation
{
	internal class Diff<TObject> : IDiff<TObject>
	{
		private readonly List<IDiffItem> aItems;

		public Diff(List<IDiffItem> items)
		{
			this.aItems = items;
		}

		#region Implementation of IEnumerable<TObject>

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

		#region Implementation of IDiff

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			foreach (IDiffItem diffItem in this)
				ret.AppendLine(diffItem.ToString(indentLevel));

			return ret.ToString().TrimEnd('\n');
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion
	}
}
