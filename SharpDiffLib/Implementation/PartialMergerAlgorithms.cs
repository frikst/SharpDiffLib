using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.Implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;

		internal PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.Algorithms = new PartialMergerAlgorithmsContainer(this.aMergerImplementation);
		}

		public PartialMergerAlgorithmsContainer Algorithms { get; }

		public IDiff<TType> Diff<TType>(TType @base, TType changed)
		{
			IDiffAlgorithm<TType> algorithm = this.Algorithms.GetDiffAlgorithm<TType>();
			return algorithm.Compute(@base, changed);
		}

		public IDiff<TType> MergeDiffs<TType>(IDiff<TType> left, IDiff<TType> right, IConflictContainer conflicts)
		{
			IMergeDiffsAlgorithm<TType> algorithm = this.Algorithms.GetMergeDiffsAlgorithm<TType>();
			return algorithm.MergeDiffs(left, right, conflicts);
		}

		public IDiff<TType> MergeDiffs<TType>(IDiff<TType> left, IDiff<TType> right, out IConflictContainer conflicts)
		{
			conflicts = new ConflictContainer();

			return this.MergeDiffs(left, right, conflicts);
		}

		public IConflictResolver<TType> GetConflictResolver<TType>(IDiff<TType> conflicted)
		{
			var conflicts = new ConflictContainer();
			conflicts.RegisterAll(conflicted);

			return new ConflictResolver<TType>(conflicted, conflicts);
		}

		public void ResolveConflicts<TType>(IConflictResolver<TType> resolver)
		{
			IResolveConflictsAlgorithm<TType> algorithm = this.Algorithms.GetResolveConflictsAlgorithm<TType>();
			algorithm.ResolveConflicts(resolver);
		}

		public TType ApplyPatch<TType>(TType @object, IDiff<TType> patch)
		{
			IApplyPatchAlgorithm<TType> algorithm = this.Algorithms.GetApplyPatchAlgorithm<TType>();
			return algorithm.Apply(@object, patch);
		}
	}
}