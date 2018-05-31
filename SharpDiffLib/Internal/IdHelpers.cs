using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Internal
{
	internal static class IdHelpers
	{
		private class IdEqualityComparer<TItemType, TIdType> : IEqualityComparer<TItemType>
		{
			private Func<TItemType, TItemType, bool> aIsTheSame;
			private Func<TItemType, TIdType> aIdAccessor;

			public IdEqualityComparer(Property idProperty)
			{
				this.aIsTheSame = CompileIsTheSame<TItemType>(idProperty);
				this.aIdAccessor = CreateIdAccessor<TItemType, TIdType>(idProperty);
			}

			#region Implementation of IEqualityComparer<in TItemType>

			bool IEqualityComparer<TItemType>.Equals(TItemType x, TItemType y)
			{
				return this.aIsTheSame(x, y);
			}

			int IEqualityComparer<TItemType>.GetHashCode(TItemType obj)
			{
				if (ReferenceEquals(obj, null))
					return 0;

				return this.aIdAccessor(obj).GetHashCode();
			}

			#endregion
		}

		public static Func<TItemType, TItemType, bool> CompileIsTheSame<TItemType>(Property idProperty)
		{
			if (idProperty == null)
			{
				return EqualityComparer<TItemType>.Default.Equals;
			}
			else
			{
				ParameterExpression baseParameter = Expression.Parameter(typeof(TItemType), "base");
				ParameterExpression changedParameter = Expression.Parameter(typeof(TItemType), "changed");

				Expression<Func<TItemType, TItemType, bool>> isTheSameExpression =
					Expression.Lambda<Func<TItemType, TItemType, bool>>(
						Expression.Condition(
							Expression.OrElse(
								Expression.ReferenceEqual(baseParameter, Expression.Constant(null)),
								Expression.ReferenceEqual(changedParameter, Expression.Constant(null))
							),
							Expression.ReferenceEqual(baseParameter, changedParameter),
							Expression.Equal(
								Expression.Property(baseParameter, idProperty.ReflectionPropertyInfo),
								Expression.Property(changedParameter, idProperty.ReflectionPropertyInfo)
							)
						),
						baseParameter, changedParameter
					);

				return isTheSameExpression.Compile();
			}
		}

		public static Func<TItemType, TIdType> CreateIdAccessor<TItemType, TIdType>(Property idProperty)
		{
			ParameterExpression obj = Expression.Parameter(typeof(TItemType), "obj");

			Expression<Func<TItemType, TIdType>> propertyGetter =
				Expression.Lambda<Func<TItemType, TIdType>>(
					Expression.Property(obj, idProperty.ReflectionPropertyInfo),
					obj
				);

			return propertyGetter.Compile();
		}

		public static IEqualityComparer<TItemType> CreateIdEqualityComparer<TItemType>(Property idProperty)
		{
			return (IEqualityComparer<TItemType>) Activator.CreateInstance(
				typeof(IdEqualityComparer<,>)
					.MakeGenericType(typeof(TItemType), idProperty.Type),
				idProperty
			);
		}
	}
}
