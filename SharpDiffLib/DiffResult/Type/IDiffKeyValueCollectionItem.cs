using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffKeyValueCollectionItem : IDiffItem
	{
		System.Type KeyType { get; }

		object Key { get; }
	}

	public interface IDiffKeyValueCollectionItem<out TKeyType> : IDiffKeyValueCollectionItem
	{
		new TKeyType Key { get; }
	}
}
