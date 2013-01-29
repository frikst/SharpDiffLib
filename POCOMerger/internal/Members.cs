using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.diffResult.type;
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

			public static MethodInfo Apply(Type tType)
			{
				return typeof(IApplyPatchAlgorithm<>).MakeGenericType(tType).GetMethod("Apply");
			}
		}

		public class MergeDiffsAlgorithm
		{
			private static readonly MethodInfo aGetAlgorithm = GetMethod<IMergeDiffsAlgorithmRules, IMergeDiffsAlgorithm>(x => x.GetAlgorithm<object>());

			public static MethodInfo GetAlgorithm(Type tType)
			{
				return aGetAlgorithm.MakeGenericMethod(tType);
			}

			public static MethodInfo MergeDiffs(Type tType)
			{
				return typeof(IMergeDiffsAlgorithm<>).MakeGenericType(tType).GetMethod("MergeDiffs");
			}
		}

		public static class DiffItems
		{
			public static ConstructorInfo NewClassChanged(Type tItemType)
			{
				return typeof(DiffClassChanged<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(Property), typeof(IDiff<>).MakeGenericType(tItemType) });
			}

			public static ConstructorInfo NewClassReplaced(Type tItemType)
			{
				return typeof(DiffClassReplaced<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(Property), tItemType, tItemType });
			}

			public static ConstructorInfo NewValueReplaced(Type tItemType)
			{
				return typeof(DiffValueReplaced<>).MakeGenericType(tItemType).GetConstructor(new[] { tItemType, tItemType });
			}

			public static ConstructorInfo NewConflict()
			{
				return typeof(DiffAnyConflicted).GetConstructor(new[] { typeof(IDiffItem), typeof(IDiffItem) });
			}

			public static PropertyInfo ClassProperty()
			{
				return typeof(IDiffClassItem).GetProperty("Property");
			}

			public static PropertyInfo ReplacedNewValue(Type tType)
			{
				return typeof(IDiffItemReplaced<>).MakeGenericType(tType).GetProperty("NewValue");
			}

			public static PropertyInfo ChangedDiff(Type tType)
			{
				return typeof(IDiffItemChanged<>).MakeGenericType(tType).GetProperty("ValueDiff");
			}
		}

		public static class Diff
		{
			public static PropertyInfo Count()
			{
				return typeof(ICountableEnumerable<IDiffItem>).GetProperty("Count");
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

		public static class FastClass
		{
			public static PropertyInfo Properties(Type tType)
			{
				return typeof(Class<>).MakeGenericType(tType).GetProperty("Properties", BindingFlags.Static | BindingFlags.Public);
			}
		}

		public static class FastProperty
		{
			public static PropertyInfo UniqueID()
			{
				return typeof(Property).GetProperty("UniqueID");
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

		public static class HashSet
		{
			public static ConstructorInfo NewHashSetFromEnumerable(Type tType)
			{
				return typeof(HashSet<>).MakeGenericType(tType).GetConstructor(new[] { typeof(IEnumerable<>).MakeGenericType(tType) });
			}

			public static MethodInfo Remove(Type tType)
			{
				return typeof(HashSet<>).MakeGenericType(tType).GetMethod("Remove");
			}
		}

		public static class Enumerable
		{
			public static MethodInfo GetEnumerator(Type tType)
			{
				return typeof(IEnumerable<>).MakeGenericType(tType).GetMethod("GetEnumerator");
			}

			public static MethodInfo MoveNext(Type tType)
			{
				return typeof(IEnumerator).GetMethod("MoveNext");
			}

			public static PropertyInfo Current(Type tType)
			{
				return typeof(IEnumerator<>).MakeGenericType(tType).GetProperty("Current");
			}
		}

		public static class Object
		{
			public static MethodInfo Equals()
			{
				return typeof(object).GetMethod("Equals", BindingFlags.Static | BindingFlags.Public);
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
