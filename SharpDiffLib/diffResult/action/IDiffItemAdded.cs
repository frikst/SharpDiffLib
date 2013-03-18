using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.action
{
	public interface IDiffItemAdded : IDiffItem
	{
		object NewValue { get; }
	}

	public interface IDiffItemAdded<TItemType> : IDiffItemAdded
	{
		new TItemType NewValue { get; }
	}
}