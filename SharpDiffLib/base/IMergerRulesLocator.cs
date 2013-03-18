using System;
using POCOMerger.definition.rules;

namespace POCOMerger.@base
{
	public interface IMergerRulesLocator
	{
		TRules GetMergerRulesFor<TRules>(Type type)
			where TRules : class, IAlgorithmRules;

		TRules GetMergerRulesForWithDefault<TRules>(Type type)
			where TRules : class, IAlgorithmRules;

		TRules GuessRules<TRules>(Type type)
			where TRules : class, IAlgorithmRules;
	}
}
