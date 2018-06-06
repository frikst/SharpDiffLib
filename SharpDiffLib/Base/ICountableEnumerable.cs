using System.Collections.Generic;

namespace KST.SharpDiffLib.Base
{
	public interface ICountableEnumerable<out T> : IEnumerable<T>
	{
		int Count { get; }
	}
}
