using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMerger.diffResult.type
{
	public interface IDiffUnorderedCollectionItemWithID
	{
		object Id { get; }
	}

	public interface IDiffUnorderedCollectionItemWithID<TIdType> : IDiffUnorderedCollectionItemWithID
	{
		Type IdType { get; }

		new TIdType Id { get; }
	}
}
