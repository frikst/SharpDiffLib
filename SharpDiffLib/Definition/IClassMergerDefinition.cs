using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Definition
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