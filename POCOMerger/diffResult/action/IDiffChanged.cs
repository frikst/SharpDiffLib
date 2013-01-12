using System.Collections.Generic;

namespace POCOMerger.diffResult.action
{
	public interface IDiffChanged : IDiffResult
	{
		IEnumerable<IDiffResult> Changes { get; }
	}
}
