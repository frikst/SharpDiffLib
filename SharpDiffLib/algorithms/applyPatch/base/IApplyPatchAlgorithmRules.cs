using SharpDiffLib.definition.rules;

namespace SharpDiffLib.algorithms.applyPatch.@base
{
	/// <summary>
	/// Rules for the patch application algorithms. Interface is designed to be used client side only
	/// and it should never be implemented directly by some class. Use generic version instead.
	/// </summary>
	public interface IApplyPatchAlgorithmRules : IAlgorithmRules
	{
		/// <summary>
		/// Creates the apply patch algorithm instance for the given object type. Every implementation
		/// of this method should test whether <typeparamref name="TType"/> type could be used with
		/// the defined rules.
		/// </summary>
		/// <typeparam name="TType">Type of the object which should be</typeparam>
		/// <returns></returns>
		IApplyPatchAlgorithm<TType> GetAlgorithm<TType>();
	}

	/// <summary>
	/// Type-safe version of the <see cref="IApplyPatchAlgorithmRules"/> interface.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which are rules defined.</typeparam>
	public interface IApplyPatchAlgorithmRules<TDefinedFor> : IApplyPatchAlgorithmRules, IAlgorithmRules<TDefinedFor>
	{

	}
}
