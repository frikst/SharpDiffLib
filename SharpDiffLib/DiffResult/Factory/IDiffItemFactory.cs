using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public interface IDiffItemFactory<out TType>
	{
		IDiff<TType> MakeDiff();
	}
}