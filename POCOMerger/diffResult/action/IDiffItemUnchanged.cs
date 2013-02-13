using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diffResult.@base;

namespace POCOMerger.diffResult.action
{
	public interface IDiffItemUnchanged : IDiffItem
	{
		object Value { get; }
	}

	public interface IDiffItemUnchanged<TItemType> : IDiffItemUnchanged
	{
		new TItemType Value { get; }
	}
}
