using System.Collections.Generic;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.action
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
