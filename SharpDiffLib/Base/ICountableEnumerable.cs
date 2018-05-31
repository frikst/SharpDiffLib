using System.Collections.Generic;

namespace SharpDiffLib.@base
{
	public interface ICountableEnumerable<T> : IEnumerable<T>
	{
		int Count { get; }
	}
}
