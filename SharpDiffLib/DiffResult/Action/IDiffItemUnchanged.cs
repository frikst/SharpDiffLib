using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
{
	public interface IDiffItemUnchanged : IDiffItem
	{
		object Value { get; }
	}

	public interface IDiffItemUnchanged<out TItemType> : IDiffItemUnchanged
	{
		new TItemType Value { get; }
	}
}
