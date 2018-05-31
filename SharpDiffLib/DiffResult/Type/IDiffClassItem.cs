using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.DiffResult.Type
{
	public interface IDiffClassItem : IDiffItem
	{
		Property Property { get; }
	}
}