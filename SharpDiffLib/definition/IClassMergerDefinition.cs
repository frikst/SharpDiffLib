using System;
using System.Collections.Generic;
using POCOMerger.definition.rules;
using POCOMerger.implementation;

namespace POCOMerger.definition
{
	internal interface IClassMergerDefinition
	{
		Type DefinedFor { get; }

		TRules GetRules<TRules>()
			where TRules : class, IAlgorithmRules;

		IEnumerable<TRules> GetAllRules<TRules>()
			where TRules : class, IAlgorithmRules;

		IAlgorithmRules GetRules(Type rulesType);

		void Initialize(MergerImplementation mergerImplementation);
	}
}