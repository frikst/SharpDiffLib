using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.diffResult.action
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
