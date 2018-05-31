using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.type
{
	public interface IDiffOrderedCollectionItem : IDiffItem
	{
		int ItemIndex { get; }
	}
}
