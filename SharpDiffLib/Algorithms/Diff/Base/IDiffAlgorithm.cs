using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.Algorithms.Diff.Base
{
	/// <summary>
	/// Algorithm for object comparison. Algorithm takes two objects and compute diff between
	/// them. This interface should not be implemented directly by any algorithm.
	/// Use <see cref="IDiffAlgorithm&lt;TType&gt;"/> instead.
	/// </summary>
	public interface IDiffAlgorithm
	{
		/// <summary>
		/// Contains true, if objects comparison defined by given algorithm can be replaced
		/// by standard equality comparison.
		/// </summary>
		bool IsDirect { get; }

		/// <summary>
		/// Computes diff between the given objects.
		/// </summary>
		/// <param name="base">Base version of the object.</param>
		/// <param name="changed">Compared version of the object.</param>
		/// <returns>Diff </returns>
		IDiff Compute(object @base, object changed);
	}

	/// <summary>
	/// Type-safe version of the <see cref="IDiffAlgorithm"/>. It is prefered to use this interface on
	/// the client side whenever possible. Every diff algorithm should implement this version of the
	/// IDiffAlgorithm.
	/// </summary>
	/// <typeparam name="TType">Type of the object to diff.</typeparam>
	public interface IDiffAlgorithm<TType> : IDiffAlgorithm
	{
		/// <summary>
		/// Computes diff between the given objects.
		/// </summary>
		/// <param name="base">Base version of the object.</param>
		/// <param name="changed">Compared version of the object.</param>
		/// <returns>Diff </returns>
		IDiff<TType> Compute(TType @base, TType changed);
	}
}