using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
{
	public interface IDiffItemReplaced : IDiffItem
	{
		object OldValue { get; }
		object NewValue { get; }
	}

	public interface IDiffItemReplaced<TItemType> : IDiffItemReplaced
	{
		new TItemType OldValue { get; }
		new TItemType NewValue { get; }
	}
}