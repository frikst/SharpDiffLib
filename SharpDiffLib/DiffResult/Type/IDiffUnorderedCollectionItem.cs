using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffUnorderedCollectionItem : IDiffItem
	{

	}

	public interface IDiffUnorderedCollectionItemWithID : IDiffUnorderedCollectionItem
	{
		object Id { get; }
	}

	public interface IDiffUnorderedCollectionItemWithID<TIdType> : IDiffUnorderedCollectionItemWithID
	{
		System.Type IdType { get; }

		new TIdType Id { get; }
	}
}
