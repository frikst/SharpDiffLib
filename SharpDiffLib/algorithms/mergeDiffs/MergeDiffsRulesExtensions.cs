using System;
using SharpDiffLib.algorithms.mergeDiffs.collection.keyValue;
using SharpDiffLib.algorithms.mergeDiffs.collection.ordered;
using SharpDiffLib.algorithms.mergeDiffs.collection.unordered;
using SharpDiffLib.algorithms.mergeDiffs.common.@class;
using SharpDiffLib.algorithms.mergeDiffs.common.value;
using SharpDiffLib.definition;

namespace SharpDiffLib.algorithms.mergeDiffs
{
	public static class MergeDiffsRulesExtensions
	{
		public static ClassMergerDefinition<TClass> MergeKeyValueCollectionDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<MergeKeyValueCollectionDiffsRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> MergeKeyValueCollectionDiffsRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<MergeKeyValueCollectionDiffsRules<TClass>>();
		}

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
