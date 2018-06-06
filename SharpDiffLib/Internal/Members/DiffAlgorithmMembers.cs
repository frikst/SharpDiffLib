using System;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.Diff.Base;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class DiffAlgorithmMembers
	{
		public static MethodInfo GetAlgorithm(Type tType)
		{
			return typeof(IDiffAlgorithmRules).GetMethod(nameof(IDiffAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
		}

		public static MethodInfo Compute(Type tType)
		{
			return typeof(IDiffAlgorithm<>).MakeGenericType(tType).GetMethod(nameof(IDiffAlgorithm<object>.Compute));
		}
	}
}
