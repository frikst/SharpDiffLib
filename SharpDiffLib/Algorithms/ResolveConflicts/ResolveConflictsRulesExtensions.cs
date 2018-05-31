using System;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.CallBack;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.DontResolve;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.ResolveAllSame;
using KST.SharpDiffLib.Definition;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts
{
	public static class ResolveConflictsRulesExtensions
	{
		public static ClassMergerDefinition<TClass> DontResolveRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<DontResolveRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> DontResolveRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<DontResolveRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ResolveAllSameRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ResolveAllSameRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ResolveAllSameRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ResolveAllSameRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ResolveByCallBackRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ResolveByCallBackRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ResolveByCallBackRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ResolveByCallBackRules<TClass>>();
		}
	}
}
