using System;
using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Definition
{
	public class ClassMergerDefinition<TClass> : IClassMergerDefinition
	{
		private readonly List<IClassMergerDefinition> aDefinitions;
		private readonly List<IAlgorithmRules<TClass>> aRules;
		private bool aInheritable;

		internal ClassMergerDefinition(List<IClassMergerDefinition> definitions)
		{
			this.aRules = new List<IAlgorithmRules<TClass>>();

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
			where TRules : class, IAlgorithmRules<TClass>, new()
		{
			TRules definition = new TRules();

			definition.InheritAfter = this.GetAncestor<TRules>(typeof(TClass));
			definition.IsInheritable = this.aInheritable;
			this.aInheritable = false;

			if (func != null)
				func(definition);

			this.aRules.Add(definition);

			return this;
		}

		public ClassMergerDefinition<TClass> Rules<TRules>()
			where TRules : class, IAlgorithmRules<TClass>, new()
		{
			return this.Rules<TRules>(null);
		}

		private IAlgorithmRules GetAncestor<TRules>(Type type)
			where TRules : class, IAlgorithmRules<TClass>
		{
			Type tmp = type;

			while (tmp.BaseType != null)
			{
				tmp = tmp.BaseType;

				foreach (IClassMergerDefinition def in this.aDefinitions)
				{
					if (def.DefinedFor == tmp)
					{
						IAlgorithmRules rules = def.GetRules(typeof(TRules));
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

		TRules IClassMergerDefinition.GetRules<TRules>(IAlgorithmRules ignore)
		{
			return this.aRules.OfType<TRules>().FirstOrDefault(x => x != ignore);
		}

		IEnumerable<TRules> IClassMergerDefinition.GetAllRules<TRules>()
		{
			return this.aRules.OfType<TRules>();
		}

		IAlgorithmRules IClassMergerDefinition.GetRules(Type rulesType)
		{
			foreach (IAlgorithmRules<TClass> rules in this.aRules)
			{
				Type currentRulesType = rules.GetType();
				if (rulesType.IsAssignableFrom(currentRulesType))
					return rules;
				else if (rulesType.IsGenericType && currentRulesType.IsGenericType && rulesType.GetGenericTypeDefinition().IsAssignableFrom(currentRulesType.GetGenericTypeDefinition()))
					return rules;
			}

			return null;
		}

		void IClassMergerDefinition.Initialize(MergerImplementation mergerImplementation)
		{
			foreach (IAlgorithmRules<TClass> algorithmRules in this.aRules)
			{
				algorithmRules.Initialize(mergerImplementation);
			}
		}

		#endregion
	}
}
