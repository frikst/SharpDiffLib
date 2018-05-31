using System;
using SharpDiffLib.algorithms.resolveConflicts.common;
using SharpDiffLib.algorithms.resolveConflicts.common.callBack;
using SharpDiffLib.algorithms.resolveConflicts.common.dontResolve;
using SharpDiffLib.algorithms.resolveConflicts.common.resolveAllSame;
using SharpDiffLib.definition;

namespace SharpDiffLib.algorithms.resolveConflicts
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
