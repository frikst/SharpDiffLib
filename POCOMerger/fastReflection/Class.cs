using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
					aProperties = new CountableEnumerableWrapper<Property>(
						typeof(TClass)
							.GetProperties()
							.Select(x => new Property(x)),
						(a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal)
					);

				return aProperties;
			}
		}
	}
}
