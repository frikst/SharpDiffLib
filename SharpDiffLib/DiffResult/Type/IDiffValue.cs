using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffValue : IDiffItem
	{
		System.Type ValueType { get; }
	}
}
