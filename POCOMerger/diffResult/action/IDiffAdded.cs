namespace POCOMerger.diffResult.action
{
	public interface IDiffAdded<T> : IDiffResult
	{
		T NewValue { get; }
	}
}