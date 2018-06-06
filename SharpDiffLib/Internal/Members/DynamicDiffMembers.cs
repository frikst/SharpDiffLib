using System;
using System.Collections.Generic;
using System.Reflection;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.Internal
{
	internal static class DynamicDiffMembers
	{
		public static ConstructorInfo New(Type tType)
		{
			return typeof(DynamicDiff<>).MakeGenericType(tType).GetConstructor(new[] { typeof(IDiff<>).MakeGenericType(tType), typeof(Dictionary<IDiffItemConflicted, ResolveAction>) });
		}

		public static MethodInfo Finish(Type tType)
		{
			return typeof(DynamicDiff<>).MakeGenericType(tType).GetMethod(nameof(DynamicDiff<object>.Finish));
		}
	}
}
