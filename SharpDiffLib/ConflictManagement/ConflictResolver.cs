using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.ConflictManagement
{
	internal class ConflictResolver<TType> : IConflictResolver<TType>
	{
		private static readonly IEqualityComparer<IDiffItemConflicted> aConflictComparer = new IdentityComparer<IDiffItemConflicted>();

		private readonly IDiff<TType> aOriginal;
		private readonly DynamicDiff<TType> aResolved;
		private readonly List<IDiffItemConflicted> aConflicts;
		private readonly Dictionary<IDiffItemConflicted, ResolveAction> aResolveActions;
		private bool aFinished;

		public ConflictResolver(IDiff<TType> original, ConflictContainer conflicts)
		{
			this.aOriginal = original;
			this.aConflicts = conflicts.GetConflicts().ToList();
			this.aResolveActions = new Dictionary<IDiffItemConflicted, ResolveAction>(this.aConflicts.Count, aConflictComparer);
			this.aResolved = new DynamicDiff<TType>(this.aOriginal, this.aResolveActions);
			this.aFinished = false;
		}

		#region Implementation of IConflictResolver

		public void ResolveConflict(IDiffItemConflicted conflict, ResolveAction resolve)
		{
			if (this.aFinished)
				throw new Exception("Cannot do this action on finished conflict resolver");
			this.aResolveActions[conflict] = resolve;
		}

		public bool HasConflicts
		{
			get
			{
				if (this.aFinished)
					throw new Exception("Cannot do this action on finished conflict resolver");
				return !this.aConflicts.All(this.aResolveActions.ContainsKey);
			}
		}

		IDiff IConflictResolver.Original
			=> this.Original;

		IDiff IConflictResolver.Resolved
			=> this.Resolved;

		public Type ObjectType
			=> typeof(TType);

		#endregion

		#region Implementation of IConflictResolver<TType>

		public IDiff<TType> Original
		{
			get
			{
				if (this.aFinished)
					throw new Exception("Cannot do this action on finished conflict resolver");
				return this.aOriginal;
			}
		}

		public IDiff<TType> Resolved
		{
			get
			{
				if (this.aFinished)
					throw new Exception("Cannot do this action on finished conflict resolver");
				return this.aResolved;
			}
		}

		public IDiff<TType> Finish()
		{
			if (this.HasConflicts)
				throw new Exception("Cannot finish conflict resolver when there are some conflicts left unresolved");

			if (this.aFinished)
				throw new Exception("Cannot do this action on finished conflict resolver");

			this.aFinished = true;

			return this.aResolved.Finish();
		}

		#endregion

		#region Implementation of IEnumerable<IDiffItemConflicted>

		public IEnumerator<IDiffItemConflicted> GetEnumerator()
		{
			return this.aConflicts.Where(x => !this.aResolveActions.ContainsKey(x)).GetEnumerator();
		}

		#endregion

		#region Implementation of IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
