using System.Collections.Generic;

namespace POCOMerger.diffResult.@base
{
	public interface IDiff : IEnumerable<IDiffItem>
	{
		string ToString(int indentLevel);
		string ToString();
	}

	public interface IDiff<TType> : IDiff
	{

	}
}
