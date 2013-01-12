namespace POCOMerger.diffResult.action
{
	public interface IDiffRemoved<T> : IDiffResult
	{
		T OldValue { get; }
	}
}