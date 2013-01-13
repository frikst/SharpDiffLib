using System.Collections.Generic;

namespace POCOMerger.diffResult
{
	public interface IDiff : IEnumerable<IDiffItem>
	{

	}

	public interface IDiff<TObject> : IDiff
	{

	}
}
