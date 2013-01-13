using System;

namespace POCOMerger.definition
{
	public interface IMergerDefinition
	{
		Type DefinedFor { get; }

		TRules GetRules<TRules>()
			where TRules : class, IAlgorithmRules;
	}
}