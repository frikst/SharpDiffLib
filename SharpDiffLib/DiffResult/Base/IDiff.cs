using KST.SharpDiffLib.Base;

namespace KST.SharpDiffLib.DiffResult.Base
{
	public interface IDiff : ICountableEnumerable<IDiffItem>
	{
		string ToString(int indentLevel);
		string ToString();

		bool HasChanges { get; }
	}

	public interface IDiff<out TType> : IDiff
	{

	}
}
