using POCOMerger.diffResult.@base;

namespace POCOMerger.algorithms.mergeDiffs.@base
{
	public interface IMergeDiffsAlgorithm
	{

	}

	public interface IMergeDiffsAlgorithm<TType> : IMergeDiffsAlgorithm
	{
		IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hasConflicts);
	}
}
