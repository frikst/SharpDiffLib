using System;
using System.Reflection;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class FastClassMembers
	{
		public static PropertyInfo Properties(Type tType)
		{
			return typeof(Class<>).MakeGenericType(tType).GetProperty(nameof(Class<object>.Properties), BindingFlags.Static | BindingFlags.Public);
		}
	}
}
