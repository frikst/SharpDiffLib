using POCOMerger.fastReflection;

namespace POCOMerger.diffResult.type
{
	public interface IDiffClassItem : IDiffItem
	{
		Property Property { get; }
	}
}