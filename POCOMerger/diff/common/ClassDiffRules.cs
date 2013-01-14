using POCOMerger.diff.@base;
using POCOMerger.implementation;

namespace POCOMerger.diff.common
{
	public class ClassDiffRules : IDiffAlgorithmRules
	{
		private MergerImplementation aMergerImplementation;

		public ClassDiffRules()
		{
			this.aMergerImplementation = null;
		}

		#region Implementation of IDiffAlgorithmRules

		public IDiffAlgorithm<TType> GetAlgorithm<TType>()
		{
			return new ClassDiff<TType>(this.aMergerImplementation);
		}

		#endregion

		#region Implementation of IAlgorithmRules

		public void Initialize(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#endregion
	}
}
