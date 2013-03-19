using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.algorithms.applyPatch.@base
{
	/// <summary>
	/// Algorithm for patch aplication. Algorithm takes an object and a diff
	/// and applies the diff to the object. This interface should not be implemented directly
	/// by any algorithm. Use <see cref="IApplyPatchAlgorithm&lt;TType&gt;"/> instead.
	/// </summary>
	public interface IApplyPatchAlgorithm
	{
		/// <summary>
		/// Applies the patch to the source. Resulting object will be other instance than the source object
		/// in the most cases. Algorithm will never change the source object.
		/// </summary>
		/// <param name="source">Source object</param>
		/// <param name="patch">Patch that should be applied to the source</param>
		/// <returns>Object after the patch application.</returns>
		object Apply(object source, IDiff patch);
	}

	/// <summary>
	/// Type-safe version of the <see cref="IApplyPatchAlgorithm"/>. It is prefered to use this interface on
	/// the client side whenever possible. Every patch algorithm should implement this version of the
	/// IApplyPatchAlgorithm.
	/// </summary>
	/// <typeparam name="TType">Type of the object to patch.</typeparam>
	public interface IApplyPatchAlgorithm<TType> : IApplyPatchAlgorithm
	{
		/// <summary>
		/// Applies the patch to the source. Resulting object will be other instance than the source object
		/// in the most cases. Algorithm will never change the source object.
		/// </summary>
		/// <param name="source">Source object</param>
		/// <param name="patch">Patch that should be applied to the source</param>
		/// <returns>Object after the patch application.</returns>
		TType Apply(TType source, IDiff<TType> patch);
	}
}
