using System;
using System.Collections;
using System.Collections.Generic;

namespace KST.SharpDiffLib.Internal
{
	internal static class LinqExtensions
	{
		public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable is ICollection collection)
			{
				Queue<T> ret = new Queue<T>(collection.Count);
				foreach (T item in collection)
					ret.Enqueue(item);

				return ret;
			}
			else
				return new Queue<T>(enumerable);
		}

		public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer)
		{
			if (first is ICollection firstCollection && second is ICollection secondCollection)
			{
				if (firstCollection.Count != secondCollection.Count)
					return false;
			}

			using (IEnumerator<T> firstEnumerator = first.GetEnumerator())
			{
				using (IEnumerator<T> secondEnumerator = second.GetEnumerator())
				{
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
	}
}
