using System;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class ApplyPatchAlgorithmMembers
	{
		public static MethodInfo GetAlgorithm(Type tType)
		{
			return typeof(IApplyPatchAlgorithmRules).GetMethod(nameof(IApplyPatchAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
		}

		public static MethodInfo Apply(Type tType)
		{
			return typeof(IApplyPatchAlgorithm<>).MakeGenericType(tType).GetMethod(nameof(IApplyPatchAlgorithm<object>.Apply));
		}
	}
}
