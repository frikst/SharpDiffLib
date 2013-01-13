using System;

namespace POCOMerger.diffResult.@base
{
	public interface IDiffItem
	{
		Type ItemType { get; }

		string ToString(int indentLevel);
		string ToString();
	}
}