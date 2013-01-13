using System.Collections.Generic;

namespace POCOMerger.diffResult.action
{
	public interface IDiffItemChanged : IDiffItem
	{
		IDiff ValueDiff { get; }
	}
}
