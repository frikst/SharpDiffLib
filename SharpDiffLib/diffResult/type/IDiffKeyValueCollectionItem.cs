using System;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.type
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
