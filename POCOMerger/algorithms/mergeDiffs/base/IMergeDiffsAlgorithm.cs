using POCOMerger.diffResult.@base;

namespace POCOMerger.algorithms.mergeDiffs.@base
{
	public interface IMergeDiffsAlgorithm
	{
		IDiff MergeDiffs(IDiff left, IDiff right, out bool hadConflicts);
	}

	public interface IMergeDiffsAlgorithm<TType> : IMergeDiffsAlgorithm
	{
		IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts);
	}
}
