using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMerger.diffResult.type
{
	public interface IDiffKeyValueCollectionItem
	{
		object Key { get; }
	}

	public interface IDiffKeyValueCollectionItem<TKeyType> : IDiffKeyValueCollectionItem
	{
		Type KeyType { get; }

		new TKeyType Key { get; }
	}
}
