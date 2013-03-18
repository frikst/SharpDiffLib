using System;
using SharpDiffLib.algorithms.applyPatch.collection.keyValue;
using SharpDiffLib.algorithms.applyPatch.collection.order;
using SharpDiffLib.algorithms.applyPatch.collection.unordered;
using SharpDiffLib.algorithms.applyPatch.common.@class;
using SharpDiffLib.algorithms.applyPatch.common.value;
using SharpDiffLib.definition;

namespace SharpDiffLib.algorithms.applyPatch
{
	public static class ApplyPatchRulesExtensions
	{
		public static ClassMergerDefinition<TClass> ApplyKeyValueCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyKeyValueCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyKeyValueCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyKeyValueCollectionPatchRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ApplyOrderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyOrderedCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyOrderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyOrderedCollectionPatchRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ApplyUnorderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyUnorderedCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyUnorderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyUnorderedCollectionPatchRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ApplyClassPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyClassPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyClassPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyClassPatchRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ApplyValuePatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyValuePatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyValuePatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyValuePatchRules<TClass>>();
		}
	}
}
