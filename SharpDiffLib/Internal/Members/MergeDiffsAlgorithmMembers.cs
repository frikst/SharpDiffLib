using System;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class MergeDiffsAlgorithmMembers
	{
		public static MethodInfo GetAlgorithm(Type tType)
		{
			return typeof(IMergeDiffsAlgorithmRules).GetMethod(nameof(IMergeDiffsAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
		}

		public static MethodInfo MergeDiffs(Type tType)
		{
			return typeof(IMergeDiffsAlgorithm<>).MakeGenericType(tType).GetMethod(nameof(IMergeDiffsAlgorithm<object>.MergeDiffs));
		}
	}
}
