using System;
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

		void IAlgorithmRules.Initialize(MergerImplementation mergerImplementation)
		{
			this.MergerImplementation = mergerImplementation;
		}

		bool IAlgorithmRules.IsInheritable { get; set; }

		IAlgorithmRules IAlgorithmRules.InheritAfter { get; set; }

		#endregion
	}
}
