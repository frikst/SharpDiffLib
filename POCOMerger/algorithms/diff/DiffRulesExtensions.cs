using System;
using POCOMerger.algorithms.diff.collection.keyValue;
using POCOMerger.algorithms.diff.collection.ordered;
using POCOMerger.algorithms.diff.collection.unordered;
using POCOMerger.algorithms.diff.common.baseClass;
using POCOMerger.algorithms.diff.common.@class;
using POCOMerger.algorithms.diff.common.value;
using POCOMerger.definition;

namespace POCOMerger.algorithms.diff
{
	public static class DiffRulesExtensions
	{
		public static ClassMergerDefinition<TClass> KeyValueCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<KeyValueCollectionDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> KeyValueCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<KeyValueCollectionDiffRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> OrderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<OrderedCollectionDiffRules<TClass>> func)
		{
			return  definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> OrderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<OrderedCollectionDiffRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> UnorderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<UnorderedCollectionDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> UnorderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<UnorderedCollectionDiffRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ClassDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ClassDiffRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ValueDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ValueDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ValueDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ValueDiffRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> BaseClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<BaseClassDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> BaseClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<BaseClassDiffRules<TClass>>();
		}
	}
}
