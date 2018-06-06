using System;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class ResolveConflictsAlgorithmMembers
	{
		public static MethodInfo GetAlgorithm(Type tType)
		{
			return typeof(IResolveConflictsAlgorithmRules).GetMethod(nameof(IResolveConflictsAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
		}
	}
}
