using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Base
{
	public interface IMergeDiffsAlgorithmRules : IAlgorithmRules
	{
		IMergeDiffsAlgorithm<TType> GetAlgorithm<TType>();
	}

	public interface IMergeDiffsAlgorithmRules<TDefinedFor> : IMergeDiffsAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
