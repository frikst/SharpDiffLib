using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Internal
{
	internal static class Members
	{
		public static class DiffAlgorithm
		{
			public static MethodInfo GetAlgorithm(Type tType)
			{
				return typeof(IDiffAlgorithmRules).GetMethod(nameof(IDiffAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
			}

			public static MethodInfo Compute(Type tType)
			{
				return typeof(IDiffAlgorithm<>).MakeGenericType(tType).GetMethod(nameof(IDiffAlgorithm<object>.Compute));
			}
		}

		public static class ApplyPatchAlgorithm
		{
			public static MethodInfo GetAlgorithm(Type tType)
			{
				return typeof(IApplyPatchAlgorithmRules).GetMethod(nameof(IApplyPatchAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
			}

			public static MethodInfo Apply(Type tType)
			{
				return typeof(IApplyPatchAlgorithm<>).MakeGenericType(tType).GetMethod(nameof(IApplyPatchAlgorithm<object>.Apply));
			}
		}

		public static class MergeDiffsAlgorithm
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

		public static class ResolveConflictsAlgorithm
		{
			public static MethodInfo GetAlgorithm(Type tType)
			{
				return typeof(IResolveConflictsAlgorithmRules).GetMethod(nameof(IResolveConflictsAlgorithmRules.GetAlgorithm)).MakeGenericMethod(tType);
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

			public static ConstructorInfo NewClassUnchanged(Type tItemType)
			{
				return typeof(DiffClassUnchanged<>).MakeGenericType(tItemType).GetConstructor(new[] { typeof(Property), tItemType });
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
				return typeof(IDiffClassItem).GetProperty(nameof(IDiffClassItem.Property));
			}

			public static PropertyInfo ReplacedNewValue(Type tType)
			{
				return typeof(IDiffItemReplaced<>).MakeGenericType(tType).GetProperty(nameof(IDiffItemReplaced<object>.NewValue));
			}

			public static PropertyInfo ChangedDiff(Type tType)
			{
				return typeof(IDiffItemChanged<>).MakeGenericType(tType).GetProperty(nameof(IDiffItemChanged<object>.ValueDiff));
			}

			public static MethodInfo IsSame()
			{
				return typeof(IDiffItem).GetMethod(nameof(IDiffItem.IsSame));
			}

			public static MethodInfo ReplaceDiffWith(Type tType)
			{
				return typeof(IDiffItemChanged<>).MakeGenericType(tType).GetMethod(nameof(IDiffItemChanged<object>.ReplaceWith));
			}
		}

		public static class Diff
		{
			public static PropertyInfo Count()
			{
				return typeof(ICountableEnumerable<IDiffItem>).GetProperty(nameof(ICountableEnumerable<object>.Count));
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
				return typeof(List<>).MakeGenericType(tItemType).GetMethod(nameof(List<object>.Add), BindingFlags.Public | BindingFlags.Instance);
			}
		}

		public static class MergerAlgorithms
		{
			public static MethodInfo Diff(Type tType)
			{
				return typeof(PartialMergerAlgorithms).GetMethod(nameof(PartialMergerAlgorithms.Diff)).MakeGenericMethod(tType);
			}
		}

		public static class ConflictContainer
		{
			public static MethodInfo RegisterConflict()
			{
				return typeof(IConflictContainer).GetMethod(nameof(IConflictContainer.RegisterConflict));
			}
		}

		public static class FastClass
		{
			public static PropertyInfo Properties(Type tType)
			{
				return typeof(Class<>).MakeGenericType(tType).GetProperty(nameof(Class<object>.Properties), BindingFlags.Static | BindingFlags.Public);
			}
		}

		public static class FastProperty
		{
			public static PropertyInfo UniqueID()
			{
				return typeof(Property).GetProperty(nameof(Property.UniqueID));
			}
		}

		public static class EqualityComparerImplementation
		{
			public static MethodInfo Equals(Type tType)
			{
				return typeof(IEqualityComparer<>).MakeGenericType(tType).GetMethod(nameof(IEqualityComparer<object>.Equals));
			}

			public static PropertyInfo Default(Type tType)
			{
				return typeof(EqualityComparer<>).MakeGenericType(tType).GetProperty(nameof(EqualityComparer<object>.Default), BindingFlags.Static | BindingFlags.Public);
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
				return typeof(HashSet<>).MakeGenericType(tType).GetMethod(nameof(HashSet<object>.Remove));
			}
		}

		public static class Enumerable
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

		public static class Object
		{
			public static MethodInfo Equals()
			{
				return typeof(object).GetMethod(nameof(object.Equals), BindingFlags.Static | BindingFlags.Public);
			}
		}

		public static class DynamicDiffMembers
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
}
