using System.Collections.Generic;

namespace KST.SharpDiffLib.Base
{
	public interface ICountableEnumerable<T> : IEnumerable<T>
	{
		int Count { get; }
	}
}
