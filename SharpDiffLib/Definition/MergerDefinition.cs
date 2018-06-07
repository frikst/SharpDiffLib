using System;
using System.Collections.Generic;
using System.Reflection;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Definition
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

					aMerger = new MergerImplementation(
						definitions, definition as IRulesNotFoundFallback
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
	}
}
