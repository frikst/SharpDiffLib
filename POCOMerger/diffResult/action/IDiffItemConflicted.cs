using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.action
{
	public interface IDiffItemConflicted : IDiffItem
	{
		IDiffItem Left { get; }

		IDiffItem Right { get; }
	}
}
