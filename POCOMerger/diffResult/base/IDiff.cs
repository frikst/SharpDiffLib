using System.Collections.Generic;
using POCOMerger.@base;

namespace POCOMerger.diffResult.@base
{
	public interface IDiff : ICountableEnumerable<IDiffItem>
	{
		string ToString(int indentLevel);
		string ToString();
	}

	public interface IDiff<out TType> : IDiff
	{

	}
}
