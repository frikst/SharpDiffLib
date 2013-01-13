using POCOMerger.diff.@base;

namespace POCOMerger.diff.common
{
	public class ClassDiffRules : IDiffAlgorithmRules
	{
		#region Implementation of IDiffAlgorithmRules

		public IDiffAlgorithm<TType> GetAlgorithm<TType>()
		{
			return new ClassDiff<TType>();
		}

		#endregion
	}
}
