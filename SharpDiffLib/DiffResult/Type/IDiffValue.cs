using System;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.type
{
	public interface IDiffValue : IDiffItem
	{
		Type ValueType { get; }
	}
}
