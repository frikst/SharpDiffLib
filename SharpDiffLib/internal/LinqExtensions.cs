using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDiffLib.@internal
{
	public static class LinqExtensions
	{
		public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
		{
			if (collection is ICollection)
			{
				Queue<T> ret = new Queue<T>(((ICollection) collection).Count);
				foreach (T item in collection)
					ret.Enqueue(item);

				return ret;
			}
			else
				return new Queue<T>(collection);
		}

		public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer)
		{
			if (first is ICollection && second is ICollection)
			{
				if (((ICollection) first).Count != ((ICollection) second).Count)
					return false;
			}

			IEnumerator<T> firstEnumerator = first.GetEnumerator();
			IEnumerator<T> secondEnumerator = second.GetEnumerator();

			while (firstEnumerator.MoveNext())
			{
				if (!secondEnumerator.MoveNext())
					return false;

				if (!comparer(firstEnumerator.Current, secondEnumerator.Current))
					return false;
			}

			return !secondEnumerator.MoveNext();
		}
	}
}
