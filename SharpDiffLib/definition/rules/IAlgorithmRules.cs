using System;
using System.Collections.Generic;
using POCOMerger.implementation;

namespace POCOMerger.definition.rules
{
	public interface IAlgorithmRules
	{
		IEnumerable<Type> GetPossibleResults();

		void Initialize(MergerImplementation mergerImplementation);
		bool IsInheritable { get; set; }
		IAlgorithmRules InheritAfter { get; set; }
	}

	public interface IAlgorithmRules<TDefinedFor> : IAlgorithmRules
	{

	}
}
