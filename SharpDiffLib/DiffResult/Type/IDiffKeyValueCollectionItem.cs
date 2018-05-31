using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffKeyValueCollectionItem : IDiffItem
	{
		object Key { get; }
	}

	public interface IDiffKeyValueCollectionItem<TKeyType> : IDiffKeyValueCollectionItem
	{
		System.Type KeyType { get; }

		new TKeyType Key { get; }
	}
}
