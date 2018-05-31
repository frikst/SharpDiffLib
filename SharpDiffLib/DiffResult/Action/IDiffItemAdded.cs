using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
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