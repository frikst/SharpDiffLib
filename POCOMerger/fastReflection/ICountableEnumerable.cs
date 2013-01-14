using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMerger.fastReflection
{
	public interface ICountableEnumerable<T> : IEnumerable<T>
	{
		int Count { get; }
	}
}
