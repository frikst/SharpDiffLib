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
		private Dictionary<Type, IDiffAlgorithm> aDiffAlgorithms;

		public PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aDiffAlgorithms = new Dictionary<Type, IDiffAlgorithm>();
			this.aMergerImplementation = mergerImplementation;
		}

		public IDiffAlgorithm<TType> GetDiffAlgorithm<TType>()
		{
			return (IDiffAlgorithm<TType>) this.GetDiffAlgorithm(typeof(TType));
		}

		private IDiffAlgorithm GetDiffAlgorithm(Type type)
		{
			IDiffAlgorithm ret;

			if (this.aDiffAlgorithms.TryGetValue(type, out ret))
				return ret;

			IDiffAlgorithmRules rules = this.aMergerImplementation.GetMergerRulesForWithDefault<IDiffAlgorithmRules>(type);

			if (rules == null)
			{
				this.aDiffAlgorithms[type] = null;
				return null;
			}

			ret = (IDiffAlgorithm) Members.DiffAlgorithm.GetAlgorithm(type).Invoke(rules, null);

			aDiffAlgorithms[type] = ret;

			return ret;
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