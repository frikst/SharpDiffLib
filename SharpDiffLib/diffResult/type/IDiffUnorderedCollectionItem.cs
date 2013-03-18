using System;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.type
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
		Type IdType { get; }

		new TIdType Id { get; }
	}
}
