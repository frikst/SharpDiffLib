using System;
using System.Collections.Generic;
using POCOMerger.implementation;

namespace POCOMerger.definition.rules
{
	public abstract class BaseRules : IAlgorithmRules
	{
		protected BaseRules()
		{
			this.MergerImplementation = null;
		}

		protected MergerImplementation MergerImplementation { get; private set; }

		#region Implementation of IAlgorithmRules

		public abstract IEnumerable<Type> GetPossibleResults();

		void IAlgorithmRules.Initialize(MergerImplementation mergerImplementation)
		{
			this.MergerImplementation = mergerImplementation;
		}

		bool IAlgorithmRules.IsInheritable { get; set; }

		IAlgorithmRules IAlgorithmRules.InheritAfter { get; set; }

		#endregion
	}
}
