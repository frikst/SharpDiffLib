using System;
using POCOMerger.definition;
using POCOMerger.diff.collection.keyValue;
using POCOMerger.diff.collection.ordered;
using POCOMerger.diff.collection.unordered;
using POCOMerger.diff.common.@class;
using POCOMerger.diff.common.value;

namespace POCOMerger.diff
{
	public static class DiffRulesExtensions
	{
		public static ClassMergerDefinition<TClass> KeyValueCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<KeyValueCollectionDiffRules> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> KeyValueCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<KeyValueCollectionDiffRules>();
		}

		public static ClassMergerDefinition<TClass> OrderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<OrderedCollectionDiffRules> func)
		{
			return  definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> OrderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<OrderedCollectionDiffRules>();
		}

		public static ClassMergerDefinition<TClass> UnorderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<UnorderedCollectionDiffRules> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> UnorderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<UnorderedCollectionDiffRules>();
		}

		public static ClassMergerDefinition<TClass> ClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ClassDiffRules> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ClassDiffRules>();
		}

		public static ClassMergerDefinition<TClass> ValueDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ValueDiffRules> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ValueDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ValueDiffRules>();
		}
	}
}
