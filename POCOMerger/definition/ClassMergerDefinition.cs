using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.implementation;

namespace POCOMerger.definition
{
	public class ClassMergerDefinition<TClass> : IClassMergerDefinition
	{
		private readonly List<IAlgorithmRules> aRules;

		public ClassMergerDefinition()
		{
			aRules = new List<IAlgorithmRules>();
		}

		public ClassMergerDefinition<TClass> Rules<TRules>(Action<TRules> func)
			where TRules : IAlgorithmRules, new()
		{
			TRules definition = new TRules();
			func(definition);
			aRules.Add(definition);

			return this;
		}

		public ClassMergerDefinition<TClass> Rules<TRules>()
			where TRules : IAlgorithmRules, new()
		{
			this.aRules.Add(new TRules());

			return this;
		}

		#region Implementation of IClassMergerDefinition

		Type IClassMergerDefinition.DefinedFor
		{
			get { return typeof(TClass); }
		}

		TRules IClassMergerDefinition.GetRules<TRules>()
		{
			return this.aRules.OfType<TRules>().FirstOrDefault();
		}

		void IClassMergerDefinition.Initialize(MergerImplementation mergerImplementation)
		{
			foreach (IAlgorithmRules algorithmRules in aRules)
			{
				algorithmRules.Initialize(mergerImplementation);
			}
		}

		#endregion
	}
}
