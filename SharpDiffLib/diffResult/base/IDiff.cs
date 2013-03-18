using System.Collections.Generic;
using SharpDiffLib.@base;

namespace SharpDiffLib.diffResult.@base
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
