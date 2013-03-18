using POCOMerger.@base;
using POCOMerger.conflictManagement;
using POCOMerger.diffResult.@base;

namespace POCOMerger.algorithms.mergeDiffs.@base
{
	public interface IMergeDiffsAlgorithm
	{
		IDiff MergeDiffs(IDiff left, IDiff right, IConflictContainer conflicts);
	}

	public interface IMergeDiffsAlgorithm<TType> : IMergeDiffsAlgorithm
	{
		IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts);
	}
}
