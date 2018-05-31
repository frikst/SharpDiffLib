using System;
using System.Collections.Generic;
using SharpDiffLib.definition.rules;
using SharpDiffLib.implementation;

namespace SharpDiffLib.definition
{
	internal interface IClassMergerDefinition
	{
		Type DefinedFor { get; }

		TRules GetRules<TRules>(IAlgorithmRules ignore)
			where TRules : class, IAlgorithmRules;

		IEnumerable<TRules> GetAllRules<TRules>()
			where TRules : class, IAlgorithmRules;

		IAlgorithmRules GetRules(Type rulesType);

		void Initialize(MergerImplementation mergerImplementation);
	}
}