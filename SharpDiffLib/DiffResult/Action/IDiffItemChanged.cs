using System.Collections.Generic;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.action
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
