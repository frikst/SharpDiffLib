using System;
using System.Collections.Generic;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Internal.Members;

namespace KST.SharpDiffLib.Implementation
{
	public class PartialMergerAlgorithmsContainer
	{
		private readonly MergerImplementation aMergerImplementation;

		private readonly Dictionary<Tuple<Type, IAlgorithmRules>, IDiffAlgorithm> aDiffAlgorithms;
		private readonly Dictionary<Tuple<Type, IAlgorithmRules>, IApplyPatchAlgorithm> aApplyPatchAlgorithms;
		private readonly Dictionary<Tuple<Type, IAlgorithmRules>, IMergeDiffsAlgorithm> aMergeDiffsAlgorithms;
		private readonly Dictionary<Tuple<Type, IAlgorithmRules>, IResolveConflictsAlgorithm> aResolveConflictsAlgorithms;

		internal PartialMergerAlgorithmsContainer(MergerImplementation mergerImplementation)
		{
			this.aDiffAlgorithms = new Dictionary<Tuple<Type, IAlgorithmRules>, IDiffAlgorithm>();
			this.aApplyPatchAlgorithms = new Dictionary<Tuple<Type, IAlgorithmRules>, IApplyPatchAlgorithm>();
			this.aMergeDiffsAlgorithms = new Dictionary<Tuple<Type, IAlgorithmRules>, IMergeDiffsAlgorithm>();
			this.aResolveConflictsAlgorithms = new Dictionary<Tuple<Type, IAlgorithmRules>, IResolveConflictsAlgorithm>();

			this.aMergerImplementation = mergerImplementation;
		}

		public IDiffAlgorithm<TType> GetDiffAlgorithm<TType>(IAlgorithmRules ignore = null)
		{
			return (IDiffAlgorithm<TType>)this.GetDiffAlgorithm(typeof(TType), ignore);
		}

		public IDiffAlgorithm GetDiffAlgorithm(Type type, IAlgorithmRules ignore = null)
		{
			return this.GetAlgorithmHelper<IDiffAlgorithm, IDiffAlgorithmRules>(type, this.aDiffAlgorithms, DiffAlgorithmMembers.GetAlgorithm(type), ignore);
		}

		public IMergeDiffsAlgorithm<TType> GetMergeDiffsAlgorithm<TType>(IAlgorithmRules ignore = null)
		{
			return (IMergeDiffsAlgorithm<TType>)this.GetMergeDiffsAlgorithm(typeof(TType), ignore);
		}

		public IMergeDiffsAlgorithm GetMergeDiffsAlgorithm(Type type, IAlgorithmRules ignore = null)
		{
			return this.GetAlgorithmHelper<IMergeDiffsAlgorithm, IMergeDiffsAlgorithmRules>(type, this.aMergeDiffsAlgorithms, MergeDiffsAlgorithmMembers.GetAlgorithm(type), ignore);
		}

		public IResolveConflictsAlgorithm<TType> GetResolveConflictsAlgorithm<TType>(IAlgorithmRules ignore = null)
		{
			return (IResolveConflictsAlgorithm<TType>)this.GetResolveConflictsAlgorithm(typeof(TType), ignore);
		}

		public IResolveConflictsAlgorithm GetResolveConflictsAlgorithm(Type type, IAlgorithmRules ignore = null)
		{
			return this.GetAlgorithmHelper<IResolveConflictsAlgorithm, IResolveConflictsAlgorithmRules>(type, this.aResolveConflictsAlgorithms, ResolveConflictsAlgorithmMembers.GetAlgorithm(type), ignore);
		}

		public IApplyPatchAlgorithm<TType> GetApplyPatchAlgorithm<TType>(IAlgorithmRules ignore = null)
		{
			return (IApplyPatchAlgorithm<TType>)this.GetApplyPatchAlgorithm(typeof(TType), ignore);
		}

		public IApplyPatchAlgorithm GetApplyPatchAlgorithm(Type type, IAlgorithmRules ignore = null)
		{
			return this.GetAlgorithmHelper<IApplyPatchAlgorithm, IApplyPatchAlgorithmRules>(type, this.aApplyPatchAlgorithms, ApplyPatchAlgorithmMembers.GetAlgorithm(type), ignore);
		}

		private TAlgorithm GetAlgorithmHelper<TAlgorithm, TAlgorithmRules>(Type type, Dictionary<Tuple<Type, IAlgorithmRules>, TAlgorithm> algorithms, MethodInfo method, IAlgorithmRules ignore)
			where TAlgorithmRules : class, IAlgorithmRules
			where TAlgorithm : class
		{
			Tuple<Type, IAlgorithmRules> algorihmKey = Tuple.Create(type, ignore);

			TAlgorithm ret;
			if (algorithms.TryGetValue(algorihmKey, out ret))
				return ret;

			TAlgorithmRules rules = this.aMergerImplementation.GetMergerRulesForWithDefault<TAlgorithmRules>(type, ignore);

			if (rules == null)
			{
				algorithms[algorihmKey] = null;
				return ret;
			}

			ret = (TAlgorithm)method.Invoke(rules, null);

			algorithms[algorihmKey] = ret;
			return ret;
		}
	}
}