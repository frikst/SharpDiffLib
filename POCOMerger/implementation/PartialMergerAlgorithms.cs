using System;
using System.Collections.Generic;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.@internal;

namespace POCOMerger.implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Dictionary<Type, IDiffAlgorithm> aDiffAlgorithms;
		private readonly Dictionary<Type, IApplyPatchAlgorithm> aApplyPatchAlgorithms;

		public PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aDiffAlgorithms = new Dictionary<Type, IDiffAlgorithm>();
			this.aApplyPatchAlgorithms = new Dictionary<Type, IApplyPatchAlgorithm>();
			this.aMergerImplementation = mergerImplementation;
		}

		public IDiffAlgorithm<TType> GetDiffAlgorithm<TType>()
		{
			return (IDiffAlgorithm<TType>) this.GetDiffAlgorithm(typeof(TType));
		}

		public IDiffAlgorithm GetDiffAlgorithm(Type type)
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

		public IApplyPatchAlgorithm<TType> GetApplyPatchAlgorithm<TType>()
		{
			return (IApplyPatchAlgorithm<TType>) this.GetApplyPatchAlgorithm(typeof(TType));
		}

		public IApplyPatchAlgorithm GetApplyPatchAlgorithm(Type type)
		{
			IApplyPatchAlgorithm ret;

			if (this.aApplyPatchAlgorithms.TryGetValue(type, out ret))
				return ret;

			IApplyPatchAlgorithmRules rules = this.aMergerImplementation.GetMergerRulesForWithDefault<IApplyPatchAlgorithmRules>(type);

			if (rules == null)
			{
				this.aDiffAlgorithms[type] = null;
				return null;
			}

			ret = (IApplyPatchAlgorithm)Members.ApplyPatchAlgorithm.GetAlgorithm(type).Invoke(rules, null);

			aApplyPatchAlgorithms[type] = ret;

			return ret;
		}

		public TType ApplyPatch<TType>(TType @object, IDiff<TType> patch)
		{
			IApplyPatchAlgorithm<TType> algorithm = this.GetApplyPatchAlgorithm<TType>();
			return algorithm.Apply(@object, patch);
		}
	}
}