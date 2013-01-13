using System.Collections.Generic;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.action
{
	public interface IDiffItemChanged : IDiffItem
	{
		IDiff ValueDiff { get; }
	}
}
