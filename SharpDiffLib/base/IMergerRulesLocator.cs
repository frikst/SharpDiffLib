using System;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.@base
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
