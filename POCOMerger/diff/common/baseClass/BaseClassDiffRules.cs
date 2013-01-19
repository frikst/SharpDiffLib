using System;
using System.Collections.Generic;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;

namespace POCOMerger.diff.common.baseClass
{
	public class BaseClassDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new BaseClassDiff<TType>(this.MergerImplementation);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			throw new NotImplementedException("I dont know how to implement this for now, but I dont need it yet.");
		}

		#endregion
	}
}
