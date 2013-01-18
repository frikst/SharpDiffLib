using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMerger.definition.rules;
using POCOMerger.implementation;

namespace POCOMerger.definition
{
	public class MergerDefinition<TDefinition>
		where TDefinition : MergerDefinition<TDefinition>
	{
		private static MergerImplementation aMerger;
		private bool aFinished;
		private readonly List<IClassMergerDefinition> aDefinitions;

		protected MergerDefinition()
		{
			this.aDefinitions = new List<IClassMergerDefinition>();
			this.aFinished = false;
		}

		public static MergerImplementation Instance
		{
			get
			{
				if (aMerger == null)
				{
					Type mapDefType = typeof(TDefinition);

					ConstructorInfo ci = mapDefType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);
					MergerDefinition<TDefinition> definition = (MergerDefinition<TDefinition>)ci.Invoke(null);
					definition.aFinished = true;

					List<IClassMergerDefinition> definitions = definition.aDefinitions;

					Func<Type, Type, IAlgorithmRules> rulesNotFoundFallback = null;

					MethodInfo rulesNotFoundFallbackMethod = definition.GetType().GetMethod("RulesNotFoundFallback", BindingFlags.NonPublic | BindingFlags.Instance);

					if (rulesNotFoundFallbackMethod != null && rulesNotFoundFallbackMethod.DeclaringType != typeof(MergerDefinition<TDefinition>))
						rulesNotFoundFallback = (rules, type) => (IAlgorithmRules) rulesNotFoundFallbackMethod.MakeGenericMethod(rules, type).Invoke(definition, null);

					aMerger = new MergerImplementation(
						definitions, rulesNotFoundFallback
					);
				}

				return aMerger;
			}
		}

		protected ClassMergerDefinition<TClass> Define<TClass>()
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			ClassMergerDefinition<TClass> ret = new ClassMergerDefinition<TClass>(this.aDefinitions);
			this.aDefinitions.Add(ret);
			return ret;
		}

		protected virtual TAlgorithmRules RulesNotFoundFallback<TAlgorithmRules, TType>()
			where TAlgorithmRules : class, IAlgorithmRules
		{
			throw new InvalidOperationException("Method is supposed to be overriden if needed");
		}
	}
}
