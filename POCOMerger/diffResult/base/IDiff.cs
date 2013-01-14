using System.Collections.Generic;

namespace POCOMerger.diffResult.@base
{
	public interface IDiff : IEnumerable<IDiffItem>
	{
		string ToString(int indentLevel);
		string ToString();

		int Count { get; }
	}

	public interface IDiff<TType> : IDiff
	{

	}
}
