using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.action
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