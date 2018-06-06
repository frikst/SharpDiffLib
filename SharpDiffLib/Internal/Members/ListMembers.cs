using System;
using System.Collections.Generic;
using System.Reflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class ListMembers
	{
		public static ConstructorInfo NewWithCount(Type tItemType)
		{
			return typeof(List<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(int) });
		}

		public static MethodInfo Add(Type tItemType)
		{
			return typeof(List<>).MakeGenericType(tItemType).GetMethod(nameof(List<object>.Add), BindingFlags.Public | BindingFlags.Instance);
		}
	}
}
