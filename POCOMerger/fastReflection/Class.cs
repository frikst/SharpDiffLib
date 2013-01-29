﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using POCOMerger.@base;
using POCOMerger.@internal;

namespace POCOMerger.fastReflection
{
	public static class Class<TClass>
	{
		private class CountableEnumerableWrapper<T> : ICountableEnumerable<T>
		{
			private readonly List<T> aCollection;

			public CountableEnumerableWrapper(IEnumerable<T> collection, Comparison<T> compare)
			{
				this.aCollection = collection.ToList();
				this.aCollection.Sort(compare);
			}

			#region Implementation of IEnumerable

			public IEnumerator<T> GetEnumerator()
			{
				return this.aCollection.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			#endregion

			#region Implementation of ICountableEnumerable<T>

			public int Count
			{
				get { return this.aCollection.Count; }
			}

			#endregion
		}

		private static ICountableEnumerable<Property> aProperties = null;

		public static ICountableEnumerable<Property> Properties
		{
			get
			{
				if (aProperties == null)
				{
					IEnumerable<Property> parentProperties;

					if (typeof(TClass).BaseType != null)
						parentProperties = (IEnumerable<Property>) Members.FastClass.Properties(typeof(TClass).BaseType).GetValue(null, null);
					else
						parentProperties = Enumerable.Empty<Property>();

					aProperties = new CountableEnumerableWrapper<Property>(
						typeof(TClass)
							.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
							.Select(x => new Property(x))
							.Union(parentProperties),
						(a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal)
					);
				}

				return aProperties;
			}
		}

		public static Property GetProperty(MemberInfo member)
		{
			return Properties.FirstOrDefault(property => property.ReflectionPropertyInfo == member);
		}

		public static Type[] KeyValueParams
		{
			get
			{
				foreach (Type @interface in typeof(TClass).GetInterfaces())
				{
					if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>))
					{
						Type enumerableItemType = @interface.GetGenericArguments()[0];

						if (enumerableItemType.IsGenericType && enumerableItemType.GetGenericTypeDefinition() == typeof (KeyValuePair<,>))
							return enumerableItemType.GetGenericArguments();
					}
				}

				return null;
			}
		}

		public static Type EnumerableParam
		{
			get
			{
				foreach (Type @interface in typeof(TClass).GetInterfaces())
				{
					if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>))
					{
						return @interface.GetGenericArguments()[0];
					}
				}

				return null;
			}
		}
	}
}
