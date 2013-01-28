using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.type
{
	public interface IDiffOrderedCollectionItem : IDiffItem
	{
		int ItemIndex { get; }

		IDiffOrderedCollectionItem CreateWithDelta(int delta);
	}
}
