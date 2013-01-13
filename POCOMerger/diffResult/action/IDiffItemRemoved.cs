namespace POCOMerger.diffResult.action
{
	public interface IDiffItemRemoved : IDiffItem
	{
		object OldValue { get; }
	}

	public interface IDiffItemRemoved<TItemType> : IDiffItemRemoved
	{
		new TItemType OldValue { get; }
	}
}