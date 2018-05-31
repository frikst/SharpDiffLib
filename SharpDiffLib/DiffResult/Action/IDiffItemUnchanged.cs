using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
{
	public interface IDiffItemUnchanged : IDiffItem
	{
		object Value { get; }
	}

	public interface IDiffItemUnchanged<TItemType> : IDiffItemUnchanged
	{
		new TItemType Value { get; }
	}
}
