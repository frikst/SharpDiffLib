using SharpDiffLib.diffResult.action;

namespace SharpDiffLib.conflictManagement
{
	public interface IConflictContainer
	{
		void RegisterConflict(IDiffItemConflicted conflict);

		bool HasConflicts { get; }
	}
}
