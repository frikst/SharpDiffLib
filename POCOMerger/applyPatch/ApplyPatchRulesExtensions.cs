using System;
using POCOMerger.applyPatch.collection.order;
using POCOMerger.applyPatch.common.@class;
using POCOMerger.definition;

namespace POCOMerger.applyPatch
{
	public static class ApplyPatchRulesExtensions
	{
		public static ClassMergerDefinition<TClass> ApplyOrderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyOrderedCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyOrderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyOrderedCollectionPatchRules<TClass>>();
		}

		public static ClassMergerDefinition<TClass> ApplyClassPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyClassPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> ApplyClassPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyClassPatchRules<TClass>>();
		}
	}
}
