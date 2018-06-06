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
	}
}
