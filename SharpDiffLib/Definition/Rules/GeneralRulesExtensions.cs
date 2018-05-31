using System;

namespace KST.SharpDiffLib.Definition.Rules
{
	public static class GeneralRulesExtensions
	{

		public static ClassMergerDefinition<TClass> GeneralRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<GeneralRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		public static ClassMergerDefinition<TClass> GeneralRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<GeneralRules<TClass>>();
		}
	}
}
