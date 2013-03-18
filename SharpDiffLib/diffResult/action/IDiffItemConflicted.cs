using SharpDiffLib.@base;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.action
{
	public interface IDiffItemConflicted : IDiffItem
	{
		ICountableEnumerable<IDiffItem> Left { get; }

		ICountableEnumerable<IDiffItem> Right { get; }
	}
}
