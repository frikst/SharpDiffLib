using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diff.common
{
	public class ClassDiff<TType> : IDiffAlgorithm<TType>
	{
		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}