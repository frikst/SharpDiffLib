using System;
using System.Collections.Generic;
using System.Reflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class EqualityComparerMembers
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
}
