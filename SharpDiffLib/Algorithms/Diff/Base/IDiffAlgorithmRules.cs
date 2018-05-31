using KST.SharpDiffLib.Definition.Rules;

namespace KST.SharpDiffLib.Algorithms.Diff.Base
{
	/// <summary>
	/// Rules for diff algorithm. Interface is designed to be used client side only
	/// and it should never be implemented directly by some class. Use generic version instead.
	/// </summary>
	public interface IDiffAlgorithmRules : IAlgorithmRules
	{
		/// <summary>
		/// Creates an diff algorithm instance for the given object type. Every implementation
		/// of this method should test whether <typeparamref name="TType"/> type could be used with
		/// the defined rules.
		/// </summary>
		/// <typeparam name="TType">Type of the diffed object</typeparam>
		/// <returns>Diff algorithm instance</returns>
		IDiffAlgorithm<TType> GetAlgorithm<TType>();
	}

	/// <summary>
	/// Type-safe version of the <see cref="IDiffAlgorithmRules"/> interface.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public interface IDiffAlgorithmRules<TDefinedFor> : IDiffAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{
		
	}
}
