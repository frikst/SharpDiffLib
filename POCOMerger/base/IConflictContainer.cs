using POCOMerger.diffResult.action;

namespace POCOMerger.@base
{
	public interface IConflictContainer
	{
		void RegisterConflict(IDiffItemConflicted conflict);

		bool HasConflicts { get; }
	}
}
