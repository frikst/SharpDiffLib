using POCOMerger.@base;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.action
{
	public interface IDiffItemConflicted : IDiffItem
	{
		ICountableEnumerable<IDiffItem> Left { get; }

		ICountableEnumerable<IDiffItem> Right { get; }
	}
}
