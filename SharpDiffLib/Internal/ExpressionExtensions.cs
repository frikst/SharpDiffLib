using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SharpDiffLib.@internal
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

		public static Expression ForEach(ParameterExpression item, Expression source, Expression body)
		{
			ParameterExpression enumerator = Expression.Parameter(typeof(IEnumerator), "enumerator");
			LabelTarget forEachEnd = Expression.Label();

			return Expression.Block(
				new[] { item, enumerator },

				Expression.Assign(
					enumerator,
					Expression.Call(source, typeof(IEnumerable).GetMethod("GetEnumerator"))
				),

				Expression.Loop(
					Expression.IfThenElse(
						Expression.Call(enumerator, typeof(IEnumerator).GetMethod("MoveNext")),

						Expression.Block(
							Expression.Assign(item, Expression.Convert(Expression.Property(enumerator, "Current"), item.Type)),
							body
						),

						Expression.Break(forEachEnd)
					),
					forEachEnd
				)
			);
		}
	}
}
