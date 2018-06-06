using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Definition.Rules
{
	internal interface IGeneralRules : IAlgorithmRules
	{
		Property IdProperty { get; }
	}

	public class GeneralRules<TType> : BaseRules<TType>, IGeneralRules, IInheritableAlgorithmRules
	{
		private Property aIdProperty;
		private IAlgorithmRules aInheritAfter;

		public GeneralRules()
		{
			this.aIdProperty = null;
		}

		public void Id<TReturnType>(Expression<Func<TType, TReturnType>> property)
		{
			this.aIdProperty = Class<TType>.GetProperty(property);
		}

		#region Implementation of IGeneralRules

		Property IGeneralRules.IdProperty
		{
			get
			{
				if (this.aIdProperty == null && this.aInheritAfter is IGeneralRules)
					this.aIdProperty = ((IGeneralRules) this.aInheritAfter).IdProperty;

				return this.aIdProperty;
			}
		}

		#endregion

		#region Implementation of IAlgorithmRules

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			return null;
		}

		bool IInheritableAlgorithmRules.IsInheritable { get; set; }

		IAlgorithmRules IInheritableAlgorithmRules.InheritedFrom
		{
			get { return this.aInheritAfter; }
			set { this.aInheritAfter = value; }
		}

		#endregion
	}
}
