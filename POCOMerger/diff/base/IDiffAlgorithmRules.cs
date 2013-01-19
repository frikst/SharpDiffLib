﻿using POCOMerger.definition;
using POCOMerger.definition.rules;

namespace POCOMerger.diff.@base
{
	public interface IDiffAlgorithmRules : IAlgorithmRules
	{
		IDiffAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IDiffAlgorithmRules<TDefinedFor> : IDiffAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{
		
	}
}
