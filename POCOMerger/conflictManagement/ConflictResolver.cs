using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;

namespace POCOMerger.conflictManagement
{
	internal class ConflictResolver<TType> : IConflictResolver<TType>
	{
		public ConflictResolver(IConflictContainer conflicts)
		{
			throw new NotImplementedException();
		}

		#region Implementation of IConflictResolver

		public void ResolveConflict(IDiffItemConflicted conflict, ResolveAction resolve)
		{
			throw new NotImplementedException();
		}

		public bool HasConflicts
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region Implementation of IConflictResolver<TType>

		public IDiff<TType> Original
		{
			get { throw new NotImplementedException(); }
		}

		public IDiff<TType> Resolved
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}
