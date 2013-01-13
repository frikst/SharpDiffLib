using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using POCOMerger.diff.@base;

namespace POCOMerger.@internal
{
	internal static class InterfaceMembers
	{
		public static MethodInfo GetAlgorithm = GetMethod<IDiffAlgorithmRules, IDiffAlgorithm>(x => x.GetAlgorithm<object>());

		private static MethodInfo GetMethod<TInterface, TType>(Expression<Func<TInterface, TType>> func)
		{
			MethodInfo methodInfo = ((MethodCallExpression) func.Body).Method;

			if (methodInfo.IsGenericMethod)
				methodInfo = methodInfo.GetGenericMethodDefinition();

			return methodInfo;
		}
	}
}
