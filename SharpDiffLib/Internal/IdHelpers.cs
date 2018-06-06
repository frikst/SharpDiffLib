using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Internal
{
	internal static class IdHelpers
	{
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
	}
}
