using System.Collections.Generic;
using POCOMerger.@base;
using POCOMerger.diffResult.action;

namespace POCOMerger.conflictManagement
{
	internal class ConflictContainer : IConflictContainer
	{
		private readonly List<IDiffItemConflicted> aConflicts;

		public ConflictContainer()
		{
			this.aConflicts = new List<IDiffItemConflicted>();
		}

		#region Implementation of IConflictContainer

		public void RegisterConflict(IDiffItemConflicted conflict)
		{
			this.aConflicts.Add(conflict);
		}

		public bool HasConflicts
		{
			get { return this.aConflicts.Count > 0; }
		}

		#endregion
	}
}
