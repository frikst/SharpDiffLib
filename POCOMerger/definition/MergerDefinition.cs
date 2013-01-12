using System;
using System.Collections.Generic;
using System.Reflection;
using POCOMerger.implementation;

namespace POCOMerger.definition
{
	public class MergerDefinition<TDefinition>
		where TDefinition : MergerDefinition<TDefinition>
	{
		private static MergerImplementation aMerger;
		private bool aFinished;
		private List<IMergerDefinition> aDefinitions;

		public MergerDefinition()
		{
			this.aDefinitions = new List<IMergerDefinition>();
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

					aMerger = new MergerImplementation(
						definition.aDefinitions
					);
				}

				return aMerger;
			}
		}

		protected ClassMergerDefinition<TClass> Define<TClass>()
		{
			if (this.aFinished)
				throw new Exception("Cannot modify the mapping");

			ClassMergerDefinition<TClass> ret = new ClassMergerDefinition<TClass>();
			this.aDefinitions.Add(ret);
			return ret;
		}
	}
}
