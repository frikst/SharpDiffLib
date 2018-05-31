using System;

namespace KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.DontResolve
{
	public class NotPossibleToResolveException : Exception
	{
		public NotPossibleToResolveException()
			: base("Trying to resolve conflicts. There should be none of it.")
		{
		}
	}
}
