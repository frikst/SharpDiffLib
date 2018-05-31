using System;

namespace SharpDiffLib.algorithms.resolveConflicts.common.dontResolve
{
	public class NotPossibleToResolveException : Exception
	{
		public NotPossibleToResolveException()
			: base("Trying to resolve conflicts. There should be none of it.")
		{
		}
	}
}
