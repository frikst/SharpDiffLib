﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using POCOMerger.applyPatch.@base;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;

namespace POCOMerger.@internal
{
	internal static class Members
	{
		public static class DiffAlgorithm
		{
			private static readonly MethodInfo aGetAlgorithm = GetMethod<IDiffAlgorithmRules, IDiffAlgorithm>(x => x.GetAlgorithm<object>());

			public static MethodInfo GetAlgorithm(Type tType)
			{
				return aGetAlgorithm.MakeGenericMethod(tType);
			}

			public static MethodInfo Compute(Type tType)
			{
				return typeof(IDiffAlgorithm<>).MakeGenericType(tType).GetMethod("Compute");
			}
		}

		public static class ApplyPatchAlgorithm
		{
			private static readonly MethodInfo aGetAlgorithm = GetMethod<IApplyPatchAlgorithmRules, IApplyPatchAlgorithm>(x => x.GetAlgorithm<object>());

			public static MethodInfo GetAlgorithm(Type tType)
			{
				return aGetAlgorithm.MakeGenericMethod(tType);
			}
		}

		public static class DiffItems
		{
			public static ConstructorInfo NewClassChanged(Type tItemType)
			{
				return typeof(DiffClassChanged<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(Property), typeof(IDiff) });
			}

			public static ConstructorInfo NewClassReplaced(Type tItemType)
			{
				return typeof(DiffClassReplaced<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(Property), tItemType, tItemType });
			}

			public static ConstructorInfo NewValueReplaced(Type tItemType)
			{
				return typeof(DiffValueReplaced<>).MakeGenericType(tItemType).GetConstructor(new[] { tItemType, tItemType });
			}
		}

		public static class Diff
		{
			public static PropertyInfo Count()
			{
				return typeof(IDiff).GetProperty("Count");
			}
		}

		public static class List
		{
			public static ConstructorInfo NewWithCount(Type tItemType)
			{
				return typeof(List<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(int) });
			}

			public static MethodInfo Add(Type tItemType)
			{
				return typeof(List<>).MakeGenericType(tItemType).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
			}
		}

		public static class MergerAlgorithms
		{
			public static MethodInfo Diff(Type tType)
			{
				return typeof(PartialMergerAlgorithms).GetMethod("Diff").MakeGenericMethod(tType);
			}
		}

		public static class Class
		{
			public static PropertyInfo Properties(Type tType)
			{
				return typeof(Class<>).MakeGenericType(tType).GetProperty("Properties", BindingFlags.Static | BindingFlags.Public);
			}
		}

		public static class EqualityComparerImplementation
		{
			public static MethodInfo Equals(Type tType)
			{
				return typeof(IEqualityComparer<>).MakeGenericType(tType).GetMethod("Equals");
			}

			public static PropertyInfo Default(Type tType)
			{
				return typeof(EqualityComparer<>).MakeGenericType(tType).GetProperty("Default", BindingFlags.Static | BindingFlags.Public);
			}
		}

		private static MethodInfo GetMethod<TType, TReturnType>(Expression<Func<TType, TReturnType>> func)
		{
			MethodInfo methodInfo = ((MethodCallExpression) func.Body).Method;

			if (methodInfo.IsGenericMethod)
				methodInfo = methodInfo.GetGenericMethodDefinition();

			return methodInfo;
		}

		private static ConstructorInfo GetConstructor<TType>(Expression<Func<TType>> func)
		{
			ConstructorInfo constructorInfo = ((NewExpression)func.Body).Constructor;

			return constructorInfo;
		}
	}
}
