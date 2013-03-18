using SharpDiffLib.diffResult.@base;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.diffResult.type
{
	public interface IDiffClassItem : IDiffItem
	{
		Property Property { get; }
	}
}