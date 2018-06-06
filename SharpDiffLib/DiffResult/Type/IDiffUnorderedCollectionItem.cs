using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffUnorderedCollectionItem : IDiffItem
	{

	}

	public interface IDiffUnorderedCollectionItemWithID : IDiffUnorderedCollectionItem
	{
		System.Type IdType { get; }

		object Id { get; }
	}

	public interface IDiffUnorderedCollectionItemWithID<out TIdType> : IDiffUnorderedCollectionItemWithID
	{
		new TIdType Id { get; }
	}
}
