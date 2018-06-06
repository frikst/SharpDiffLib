using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Definition.Rules
{
	public interface IAlgorithmRules
	{
		IEnumerable<Type> GetPossibleResults();

		void Initialize(MergerImplementation mergerImplementation);
	}

	public interface IAlgorithmRules<TDefinedFor> : IAlgorithmRules
	{

	}
}
