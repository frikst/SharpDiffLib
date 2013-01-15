using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMerger.definition;
using POCOMerger.diff.@base;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;
using POCOMerger.@internal;

namespace POCOMerger.implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;

		public PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		public IDiffAlgorithm<TType> GetDiffAlgorithm<TType>()
		{
			return (IDiffAlgorithm<TType>) this.GetDiffAlgorithm(typeof(TType));
		}

		private IDiffAlgorithm GetDiffAlgorithm(Type type)
		{
			IClassMergerDefinition definition = this.aMergerImplementation.GetMergerFor(type);

			if (definition == null)
				return null;

			object rules = definition.GetRules<IDiffAlgorithmRules>();

			if (rules == null)
				return null;

			return (IDiffAlgorithm) Members.DiffAlgorithm.GetAlgorithm(type).Invoke(rules, null);
		}

		public IDiff<TType> Diff<TType>(TType @base, TType changed)
		{
			IDiffAlgorithm<TType> algorithm = this.GetDiffAlgorithm<TType>();
			return algorithm.Compute(@base, changed);
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