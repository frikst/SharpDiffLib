using System;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.type
{
	public interface IDiffKeyValueCollectionItem : IDiffItem
	{
		object Key { get; }
	}

	public interface IDiffKeyValueCollectionItem<TKeyType> : IDiffKeyValueCollectionItem
	{
		Type KeyType { get; }

		new TKeyType Key { get; }
	}
}
