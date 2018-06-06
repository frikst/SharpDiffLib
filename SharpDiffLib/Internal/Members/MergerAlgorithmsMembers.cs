using System;
using System.Reflection;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class MergerAlgorithmsMembers
	{
		public static MethodInfo Diff(Type tType)
		{
			return typeof(PartialMergerAlgorithms).GetMethod(nameof(PartialMergerAlgorithms.Diff)).MakeGenericMethod(tType);
		}
	}
}
