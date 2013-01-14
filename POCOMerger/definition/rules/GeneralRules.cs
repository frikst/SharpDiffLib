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
	public class GeneralRules<TType> : IGeneralRules
	{
		private Property aIdProperty;

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
			get { return this.aIdProperty; }
		}

		#endregion

		#region Implementation of IAlgorithmRules

		public void Initialize(MergerImplementation mergerImplementation)
		{
			
		}

		#endregion
	}

	internal interface IGeneralRules : IAlgorithmRules
	{
		Property IdProperty { get; }
	}
}
