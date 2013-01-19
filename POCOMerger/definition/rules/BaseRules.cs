using System;
using System.Collections.Generic;
using POCOMerger.implementation;

namespace POCOMerger.definition.rules
{
	public abstract class BaseRules<TDefinedFor> : IAlgorithmRules<TDefinedFor>
	{
		protected BaseRules()
		{
			this.MergerImplementation = null;
		}

		protected MergerImplementation MergerImplementation { get; private set; }

		#region Implementation of IAlgorithmRules

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			return null;
		}

		void IAlgorithmRules.Initialize(MergerImplementation mergerImplementation)
		{
			this.MergerImplementation = mergerImplementation;
		}

		bool IAlgorithmRules.IsInheritable { get; set; }

		IAlgorithmRules IAlgorithmRules.InheritAfter { get; set; }

		#endregion
	}
}
