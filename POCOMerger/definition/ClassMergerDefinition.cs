using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOMerger.definition
{
	public class ClassMergerDefinition<TClass> : IMergerDefinition
	{
		private readonly List<IAlgorithmRules> aRules;

		public ClassMergerDefinition()
		{
			aRules = new List<IAlgorithmRules>();
		}

		public void Rules<TRules>(Action<TRules> func)
			where TRules : IAlgorithmRules, new()
		{
			TRules definition = new TRules();
			func(definition);
			aRules.Add(definition);
		}

		public void Rules<TRules>()
			where TRules : IAlgorithmRules, new()
		{
			this.aRules.Add(new TRules());
		}

		#region Implementation of IMergerDefinition

		Type IMergerDefinition.DefinedFor
		{
			get { return typeof(TClass); }
		}

		TRules IMergerDefinition.GetRules<TRules>()
		{
			return this.aRules.OfType<TRules>().FirstOrDefault();
		}

		#endregion
	}
}
