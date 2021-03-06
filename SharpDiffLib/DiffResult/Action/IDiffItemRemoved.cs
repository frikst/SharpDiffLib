using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
{
	public interface IDiffItemRemoved : IDiffItem
	{
		object OldValue { get; }
	}

	public interface IDiffItemRemoved<out TItemType> : IDiffItemRemoved
	{
		new TItemType OldValue { get; }
	}
}