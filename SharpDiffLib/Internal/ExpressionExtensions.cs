using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.Internal.Members;

namespace KST.SharpDiffLib.Internal
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

				object cmp = EqualityComparerMembers.Default(left.Type).GetValue(null, null);

				if (cmp == null)
					throw;

				return Expression.Not(
					Expression.Call(
						Expression.Constant(cmp),
						EqualityComparerMembers.Equals(left.Type),
						left, right
					)
				);
			}
		}

		public static Expression ForEach(ParameterExpression item, Expression source, Expression body)
		{
			if (!typeof(IEnumerable).IsAssignableFrom(source.Type))
				throw new InvalidOperationException($"{source.Type} does not implement IEnumerable and cannot be iterated using foreach");

			ParameterExpression enumerator = Expression.Parameter(typeof(IEnumerator), "enumerator");
			LabelTarget forEachEnd = Expression.Label();

			return Expression.Block(
				new[] { item, enumerator },

				Expression.Assign(
					enumerator,
					Expression.Call(source, EnumerableMembers.GetEnumerator())
				),

				Expression.Loop(
					Expression.IfThenElse(
						Expression.Call(enumerator, EnumerableMembers.MoveNext()),

						Expression.Block(
							Expression.Assign(item, Expression.Convert(Expression.Property(enumerator, EnumerableMembers.Current()), item.Type)),
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
