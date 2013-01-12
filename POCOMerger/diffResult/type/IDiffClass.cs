using POCOMerger.fastReflection;

namespace POCOMerger.diffResult.type
{
	public interface IDiffClass : IDiffResult
	{
		Property Property { get; }
	}
}