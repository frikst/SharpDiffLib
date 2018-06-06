using System;
using System.Collections.Generic;
using System.Reflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class HashSetMembers
	{
		public static ConstructorInfo NewHashSetFromEnumerable(Type tType)
		{
			return typeof(HashSet<>).MakeGenericType(tType).GetConstructor(new[] { typeof(IEnumerable<>).MakeGenericType(tType) });
		}

		public static MethodInfo Remove(Type tType)
		{
			return typeof(HashSet<>).MakeGenericType(tType).GetMethod(nameof(HashSet<object>.Remove));
		}
	}
}
