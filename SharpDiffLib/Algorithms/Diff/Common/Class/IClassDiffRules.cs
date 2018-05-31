using System.Collections.Generic;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.diff.common.@class
{
	internal interface IClassDiffRules
	{
		IEnumerable<Property> IgnoredProperties { get; }
		IEnumerable<Property> AlwaysIncludedProperties { get; }
	}
}