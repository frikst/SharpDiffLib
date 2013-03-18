using System;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.algorithms.diff.@base;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;

		internal PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.Algorithms = new PartialMergerAlgorithmsContainer(this.aMergerImplementation);
		}

		public PartialMergerAlgorithmsContainer Algorithms { get; private set; }

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

		public void ResolveConflicts<TType>(IDiff<TType> conflicted, IConflictResolver resolver)
		{
			throw new NotImplementedException();
		}

		public TType ApplyPatch<TType>(TType @object, IDiff<TType> patch)
		{
			IApplyPatchAlgorithm<TType> algorithm = this.Algorithms.GetApplyPatchAlgorithm<TType>();
			return algorithm.Apply(@object, patch);
		}
	}
}