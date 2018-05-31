using KST.SharpDiffLib.DiffResult.Action;

namespace KST.SharpDiffLib.ConflictManagement
{
	public interface IConflictContainer
	{
		void RegisterConflict(IDiffItemConflicted conflict);

		bool HasConflicts { get; }
	}
}
