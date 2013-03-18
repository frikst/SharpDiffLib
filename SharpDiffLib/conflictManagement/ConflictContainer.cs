using System.Collections.Generic;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;

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

		public IEnumerable<IDiffItemConflicted> GetConflicts()
		{
			return this.aConflicts;
		}

		public void RegisterAll(IDiff diff)
		{
			foreach (IDiffItem item in diff)
			{
				if (item is IDiffItemConflicted)
					this.RegisterConflict((IDiffItemConflicted) item);
				else if (item is IDiffItemChanged)
					this.RegisterAll(((IDiffItemChanged) item).ValueDiff);
			}
		}
	}
}
