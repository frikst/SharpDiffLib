using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using POCOMerger.fastReflection;
using POCOMerger.implementation;

namespace POCOMerger.definition.rules
{
	internal interface IGeneralRules : IAlgorithmRules
	{
		Property IdProperty { get; }
	}

	public class GeneralRules<TType> : BaseRules, IGeneralRules
	{
		private Property aIdProperty;
		private IAlgorithmRules aInheritAfter;

		public GeneralRules()
		{
			this.aIdProperty = null;
		}

		public void Id<TReturnType>(Expression<Func<TType, TReturnType>> property)
		{
			this.aIdProperty = Class<TType>.GetProperty(((MemberExpression) property.Body).Member);
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

		IAlgorithmRules IAlgorithmRules.InheritAfter
		{
			get { return this.aInheritAfter; }
			set { this.aInheritAfter = value; }
		}

		#endregion
	}
}
