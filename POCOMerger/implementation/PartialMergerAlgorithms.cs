using System;
using System.Collections.Generic;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;

namespace POCOMerger.implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;

		public PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		public IDiff<TType> Diff<TType>(TType @base, TType changed)
		{
			throw new NotImplementedException();
		}

		public IDiff<TType> Merge<TType>(IDiff<TType> left, IDiff<TType> right)
		{
			throw new NotImplementedException();
		}

		public IDiff<TType> ResolveConflicts<TType>(IDiff<TType> conflicted)
		{
			throw new NotImplementedException();
		}

		public TType ApplyPatch<TType>(TType @object, IDiff<TType> patch)
		{
			throw new NotImplementedException();
		}
	}
}