using System;
using System.Reflection;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class DiffItemsMembers
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

		public static MethodInfo ReplaceDiffWith(Type tType)
		{
			return typeof(IDiffItemChanged<>).MakeGenericType(tType).GetMethod(nameof(IDiffItemChanged<object>.ReplaceWith));
		}
	}
}
