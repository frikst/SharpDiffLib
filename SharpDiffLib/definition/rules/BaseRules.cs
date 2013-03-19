using System;
using System.Collections.Generic;
using SharpDiffLib.implementation;

namespace SharpDiffLib.definition.rules
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
			if (((IAlgorithmRules) this).IsInheritable)
				return typeof(TDefinedFor).IsAssignableFrom(typeof(TType));
			else
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

		bool IAlgorithmRules.IsInheritable { get; set; }

		IAlgorithmRules IAlgorithmRules.InheritAfter
		{
			get { return null; }
			set { if (value != null) throw new InvalidOperationException("Cannot inherit rules for " + this.GetType().Name); }
		}

		#endregion
	}
}
