using POCOMerger.definition;
using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.common
{
	public class ValueDiffRules : IDiffAlgorithmRules
	{
		private MergerImplementation aMergerImplementation;

		public ValueDiffRules()
		{
			this.aMergerImplementation = null;
		}

		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ValueDiff<TType>(this.aMergerImplementation);
		}

		#endregion

		#region Implementation of IAlgorithmRules

		void IAlgorithmRules.Initialize(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#endregion
	}
}
