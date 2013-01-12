using System;

namespace POCOMerger.definition
{
	public class ClassMergerDefinition<TClass> : IMergerDefinition
	{
		private DiffDefinition<TClass> aDiffDefinition;

		public ClassMergerDefinition()
		{
			this.aDiffDefinition = null;
		}

		public void Diff(Action<DiffDefinition<TClass>> func)
		{
			if (this.aDiffDefinition != null)
				throw new Exception("You cannot call Diff definition twice");

			DiffDefinition<TClass> definition = new DiffDefinition<TClass>();
			func(definition);
			this.aDiffDefinition = definition;
		}
	}
}
