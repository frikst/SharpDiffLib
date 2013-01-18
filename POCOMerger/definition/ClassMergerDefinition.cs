using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.definition.rules;
using POCOMerger.implementation;

namespace POCOMerger.definition
{
	public class ClassMergerDefinition<TClass> : IClassMergerDefinition
	{
		private readonly List<IClassMergerDefinition> aDefinitions;
		private readonly List<IAlgorithmRules> aRules;
		private bool aInheritable;

		internal ClassMergerDefinition(List<IClassMergerDefinition> definitions)
		{
			this.aRules = new List<IAlgorithmRules>();

			this.aDefinitions = definitions;
			this.aInheritable = false;
		}

		public ClassMergerDefinition<TClass> Inheritable
		{
			get
			{
				this.aInheritable = true;
				return this;
			}
		}

		public ClassMergerDefinition<TClass> Rules<TRules>(Action<TRules> func)
			where TRules : class, IAlgorithmRules, new()
		{
			TRules definition = new TRules();

			definition.InheritAfter = this.GetAncestor<TRules>();
			definition.IsInheritable = this.aInheritable;
			this.aInheritable = false;

			if (func != null)
				func(definition);

			this.aRules.Add(definition);

			return this;
		}

		public ClassMergerDefinition<TClass> Rules<TRules>()
			where TRules : class, IAlgorithmRules, new()
		{
			return this.Rules<TRules>(null);
		}

		private TRules GetAncestor<TRules>()
			where TRules : class, IAlgorithmRules
		{
			Type tmp = typeof(TRules);

			while (tmp.BaseType != null)
			{
				tmp = tmp.BaseType;

				foreach (IClassMergerDefinition def in aDefinitions)
				{
					if (def.DefinedFor == tmp)
					{
						TRules rules = def.GetRules<TRules>();
						if (rules != null && rules.IsInheritable)
							return rules;
					}
				}
			}

			return null;
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

		public IEnumerable<TRules> GetAllRules<TRules>() where TRules : class, IAlgorithmRules
		{
			return this.aRules.OfType<TRules>();
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
