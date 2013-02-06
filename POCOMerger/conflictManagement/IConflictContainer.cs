using POCOMerger.diffResult.action;

namespace POCOMerger.conflictManagement
{
	public interface IConflictContainer
	{
		void RegisterConflict(IDiffItemConflicted conflict);

		bool HasConflicts { get; }
	}
}
