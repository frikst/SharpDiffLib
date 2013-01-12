namespace POCOMerger.diffResult.action
{
	public interface IDiffReplaced<T> : IDiffResult
	{
		T OldValue { get; }
		T NewValue { get; }
	}
}