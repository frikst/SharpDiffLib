using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace POCOMerger.@internal
{
	internal static class ExpressionExtensions
	{
		public static Expression NotEqual(Expression left, Expression right)
		{
			try
			{
				return Expression.NotEqual(left, right);
			}
			catch (InvalidOperationException)
			{
				if (left.Type != right.Type)
					throw;

				object cmp = Members.EqualityComparerImplementation.Default(left.Type).GetValue(null, null);

				if (cmp == null)
					throw;

				return Expression.Not(
					Expression.Call(
						Expression.Constant(cmp),
						Members.EqualityComparerImplementation.Equals(left.Type),
						left, right
					)
				);
			}
		}
	}
}
