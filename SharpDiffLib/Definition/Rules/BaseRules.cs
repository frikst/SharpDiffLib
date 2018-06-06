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
				throw new Exception($"Cannot use algorithm defined for {typeof(TDefinedFor).Name} with the {typeof(TType).Name} type");
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
