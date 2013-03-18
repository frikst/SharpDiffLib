using System.Collections.Generic;

namespace POCOMerger.@base
{
	public interface ICountableEnumerable<T> : IEnumerable<T>
	{
		int Count { get; }
	}
}
