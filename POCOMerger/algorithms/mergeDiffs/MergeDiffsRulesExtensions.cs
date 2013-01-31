using System;
using POCOMerger.algorithms.mergeDiffs.collection.ordered;
using POCOMerger.algorithms.mergeDiffs.collection.unordered;
using POCOMerger.algorithms.mergeDiffs.common.@class;
using POCOMerger.algorithms.mergeDiffs.common.value;
using POCOMerger.definition;

namespace POCOMerger.algorithms.mergeDiffs
{
	public static class MergeDiffsRulesExtensions
	{
		public static ClassMergerDefinition<TClass> MergeOrderedCollectionDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<MergeOrderedCollectionDiffsRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> MergeOrderedCollectionDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<MergeOrderedCollectionDiffsRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> MergeUnorderedCollectionDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<MergeUnorderedCollectionDiffsRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> MergeUnorderedCollectionDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<MergeUnorderedCollectionDiffsRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> MergeClassDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<MergeClassDiffsRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> MergeClassDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<MergeClassDiffsRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> MergeValueDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<MergeValueDiffsRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> MergeValueDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<MergeValueDiffsRules<TClass>>();
		}
	}
}
