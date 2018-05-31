using System;
using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Base
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
