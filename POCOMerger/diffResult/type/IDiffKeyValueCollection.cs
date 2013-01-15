using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMerger.diffResult.type
{
	public interface IDiffKeyValueCollection
	{
		object Key { get; }
	}

	public interface IDiffKeyValueCollection<TKeyType> : IDiffKeyValueCollection
	{
		Type KeyType { get; }

		new TKeyType Key { get; }
	}
}
