using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.action
{
	public interface IDiffItemRemoved : IDiffItem
	{
		object OldValue { get; }
	}

	public interface IDiffItemRemoved<TItemType> : IDiffItemRemoved
	{
		new TItemType OldValue { get; }
	}
}