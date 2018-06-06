using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Definition.Rules
{
	public abstract class BaseRules<TDefinedFor> : IAlgorithmRules<TDefinedFor>
	{
		protected BaseRules()
		{
			this.MergerImplementation = null;
		}

		protected MergerImplementation MergerImplementation { get; private set; }

		private bool CanBeUsedFor<TType>()
		{
			if (this is IInheritableAlgorithmRules inheritable)
				if (inheritable.IsInheritable)
					return typeof(TDefinedFor).IsAssignableFrom(typeof(TType));

			return typeof(TType) == typeof(TDefinedFor);
		}

		protected void ValidateType<TType>()
		{
			if (!this.CanBeUsedFor<TType>())
				throw new Exception(string.Format("Cannot use algorithm defined for {0} with the {1} type", typeof(TDefinedFor), typeof(TType)));
		}

		#region Implementation of IAlgorithmRules

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			return null;
		}

		void IAlgorithmRules.Initialize(MergerImplementation mergerImplementation)
		{
			this.MergerImplementation = mergerImplementation;
		}

		#endregion
	}
}
