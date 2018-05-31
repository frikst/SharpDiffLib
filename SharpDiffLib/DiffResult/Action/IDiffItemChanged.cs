using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
{
	public interface IDiffItemChanged : IDiffItem
	{
		IDiff ValueDiff { get; }
	}

	public interface IDiffItemChanged<TItemType> : IDiffItemChanged
	{
		new IDiff<TItemType> ValueDiff { get; }

		IDiffItemChanged<TItemType> ReplaceWith(IDiff<TItemType> diff);
	}
}
