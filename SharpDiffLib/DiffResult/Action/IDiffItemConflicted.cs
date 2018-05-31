using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.DiffResult.Action
{
	public interface IDiffItemConflicted : IDiffItem
	{
		ICountableEnumerable<IDiffItem> Left { get; }

		ICountableEnumerable<IDiffItem> Right { get; }
	}
}
