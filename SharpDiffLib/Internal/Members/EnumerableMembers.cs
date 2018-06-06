using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class EnumerableMembers
	{
		public static MethodInfo GetEnumerator(Type tType)
		{
			return typeof(IEnumerable<>).MakeGenericType(tType).GetMethod(nameof(IEnumerable<object>.GetEnumerator));
		}

		public static MethodInfo MoveNext(Type tType)
		{
			return typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext));
		}

		public static PropertyInfo Current(Type tType)
		{
			return typeof(IEnumerator<>).MakeGenericType(tType).GetProperty(nameof(IEnumerator<object>.Current));
		}

		public static MethodInfo GetEnumerator()
		{
			return typeof(IEnumerable).GetMethod(nameof(IEnumerable.GetEnumerator));
		}

		public static MethodInfo MoveNext()
		{
			return typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext));
		}

		public static PropertyInfo Current()
		{
			return typeof(IEnumerator).GetProperty(nameof(IEnumerator.Current));
		}
	}
}
