using System;
using System.Collections.Generic;
using POCOMerger.diffResult;

namespace POCOMerger.implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;

		public PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		public IEnumerable<IDiffResult> Diff<TType>(TType @base, TType changed)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IDiffResult> Merge(IEnumerable<IDiffResult> left, IEnumerable<IDiffResult> right)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IDiffResult> ResolveConflicts(IEnumerable<IDiffResult> conflicted)
		{
			throw new NotImplementedException();
		}

		public TType ApplyPatch<TType>(TType @object, IEnumerable<IDiffResult> patch)
		{
			throw new NotImplementedException();
		}
	}
}