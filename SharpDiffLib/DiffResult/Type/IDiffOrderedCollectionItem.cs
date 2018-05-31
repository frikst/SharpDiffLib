using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffOrderedCollectionItem : IDiffItem
	{
		int ItemIndex { get; }
	}
}
