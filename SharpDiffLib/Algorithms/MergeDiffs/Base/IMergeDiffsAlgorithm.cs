using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Base
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
