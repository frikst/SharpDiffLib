using System;
using POCOMerger.diff.@base;

namespace POCOMerger.definition
{
	public class DiffDefinition<TClass>
	{
		private Type aAlgorithm;

		public DiffDefinition()
		{
			this.aAlgorithm = null;
		}

		public void Using<TAlgorithm>()
			where TAlgorithm : IDiffAlgorithm
		{
			this.aAlgorithm = typeof(TAlgorithm);
		}
	}
}