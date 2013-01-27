using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.@base;
using POCOMerger.@internal;

namespace POCOMerger.implementation
{
	public class PartialMergerAlgorithms
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Dictionary<Type, IDiffAlgorithm> aDiffAlgorithms;
		private readonly Dictionary<Type, IApplyPatchAlgorithm> aApplyPatchAlgorithms;
		private readonly Dictionary<Type, IMergeDiffsAlgorithm> aMergeDiffsAlgorithms;

		public PartialMergerAlgorithms(MergerImplementation mergerImplementation)
		{
			this.aDiffAlgorithms = new Dictionary<Type, IDiffAlgorithm>();
			this.aApplyPatchAlgorithms = new Dictionary<Type, IApplyPatchAlgorithm>();
			this.aMergeDiffsAlgorithms = new Dictionary<Type, IMergeDiffsAlgorithm>();
			this.aMergerImplementation = mergerImplementation;
		}

		public IDiffAlgorithm<TType> GetDiffAlgorithm<TType>()
		{
			return (IDiffAlgorithm<TType>) this.GetDiffAlgorithm(typeof(TType));
		}

		public IDiffAlgorithm GetDiffAlgorithm(Type type)
		{
			return this.GetAlgorithmHelper<IDiffAlgorithm, IDiffAlgorithmRules>(type, this.aDiffAlgorithms, Members.DiffAlgorithm.GetAlgorithm(type));
		}

		public IDiff<TType> Diff<TType>(TType @base, TType changed)
		{
			IDiffAlgorithm<TType> algorithm = this.GetDiffAlgorithm<TType>();
			return algorithm.Compute(@base, changed);
		}

		public IMergeDiffsAlgorithm<TType> GetMergeDiffsAlgorithm<TType>()
		{
			return (IMergeDiffsAlgorithm<TType>)this.GetMergeDiffsAlgorithm(typeof(TType));
		}

		public IMergeDiffsAlgorithm GetMergeDiffsAlgorithm(Type type)
		{
			return this.GetAlgorithmHelper<IMergeDiffsAlgorithm, IMergeDiffsAlgorithmRules>(type, this.aMergeDiffsAlgorithms, Members.MergeDiffsAlgorithm.GetAlgorithm(type));
		}

		public IDiff<TType> MergeDiffs<TType>(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			IMergeDiffsAlgorithm<TType> algorithm = this.GetMergeDiffsAlgorithm<TType>();
			return algorithm.MergeDiffs(left, right, out hadConflicts);
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
			return this.GetAlgorithmHelper<IApplyPatchAlgorithm, IApplyPatchAlgorithmRules>(type, this.aApplyPatchAlgorithms, Members.ApplyPatchAlgorithm.GetAlgorithm(type));
		}

		public TType ApplyPatch<TType>(TType @object, IDiff<TType> patch)
		{
			IApplyPatchAlgorithm<TType> algorithm = this.GetApplyPatchAlgorithm<TType>();
			return algorithm.Apply(@object, patch);
		}

		private TAlgorithm GetAlgorithmHelper<TAlgorithm, TAlgorithmRules>(Type type, Dictionary<Type, TAlgorithm> algorithms, MethodInfo method)
			where TAlgorithmRules : class, IAlgorithmRules
			where TAlgorithm : class
		{
			TAlgorithm ret;
			if (algorithms.TryGetValue(type, out ret))
				return ret;

			TAlgorithmRules rules = this.aMergerImplementation.GetMergerRulesForWithDefault<TAlgorithmRules>(type);

			if (rules == null)
			{
				algorithms[type] = null;
				return ret;
			}

			ret = (TAlgorithm)method.Invoke(rules, null);

			algorithms[type] = ret;
			return ret;
		}
	}
}