using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.definition.rules;

namespace POCOMerger.definition
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
