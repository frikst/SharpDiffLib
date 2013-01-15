using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMerger.@internal
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
	}
}
