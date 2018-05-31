using System.Collections.Generic;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Algorithms.Diff.Common.Class
{
	internal interface IClassDiffRules
	{
		IEnumerable<Property> IgnoredProperties { get; }
		IEnumerable<Property> AlwaysIncludedProperties { get; }
	}
}