using System.Collections.Generic;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.ConflictManagement
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
			=> this.aConflicts.Count > 0;

		#endregion

		public IEnumerable<IDiffItemConflicted> GetConflicts()
		{
			return this.aConflicts;
		}

		public void RegisterAll(IDiff diff)
		{
			foreach (IDiffItem item in diff)
			{
				switch (item)
				{
					case IDiffItemConflicted itemConflicted:
						this.RegisterConflict(itemConflicted);
						break;
					case IDiffItemChanged itemChanged:
						this.RegisterAll(itemChanged.ValueDiff);
						break;
				}
			}
		}
	}
}
