using System;
using POCOMerger.algorithms.mergeDiffs.common.@class;
using POCOMerger.definition;

namespace POCOMerger.algorithms.mergeDiffs
{
	public static class MergeDiffsRulesExtensions
	{
		public static ClassMergerDefinition<TClass> MergeClassDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<MergeClassDiffsRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> MergeClassDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<MergeClassDiffsRules<TClass>>();
		}
	}
}
