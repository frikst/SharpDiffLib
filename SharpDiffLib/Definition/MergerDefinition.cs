using System;
using System.Collections.Generic;
using System.Reflection;
using SharpDiffLib.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition.rules;
using SharpDiffLib.implementation;

namespace SharpDiffLib.definition
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

					MethodInfo rulesNotFoundFallbackMethod = definition.GetType().GetMethod("RulesNotFoundFallback", BindingFlags.NonPublic | BindingFlags.Instance);
					Func<Type, Type, IMergerRulesLocator, IAlgorithmRules> rulesNotFoundFallback = (rules, type, rulesLocator) => (IAlgorithmRules) rulesNotFoundFallbackMethod.MakeGenericMethod(rules, type).Invoke(definition, new object[] { rulesLocator });

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

		protected virtual TAlgorithmRules RulesNotFoundFallback<TAlgorithmRules, TType>(IMergerRulesLocator rulesLocator)
			where TAlgorithmRules : class, IAlgorithmRules
		{
			return null;
		}
	}
}
