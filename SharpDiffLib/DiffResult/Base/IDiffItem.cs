namespace KST.SharpDiffLib.DiffResult.Base
{
	public interface IDiffItem
	{
		/// <summary>
		/// Compares diff action for equality, does not take item identificator
		/// (property name, index, ...) and item type (class diff, collection diff, ...)
		/// into account
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsSame(IDiffItem other);

		System.Type ItemType { get; }

		string ToString(int indentLevel);
		string ToString();
	}
}