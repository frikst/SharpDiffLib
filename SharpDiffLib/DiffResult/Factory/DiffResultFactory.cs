using System.Collections.Generic;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public static class DiffResultFactory
	{
		public static ClassDiffItemFactory<TType> Class<TType>()
		{
			return new ClassDiffItemFactory<TType>();
		}

		public static KeyValueDiffItemFactory<Dictionary<TKey, TValue>, TKey, TValue> KeyValue<TKey, TValue>()
		{
			return DiffResultFactory.KeyValue<Dictionary<TKey, TValue>, TKey, TValue>();
		}

		public static KeyValueDiffItemFactory<TType, TKey, TValue> KeyValue<TType, TKey, TValue>()
			where TType : IEnumerable<KeyValuePair<TKey, TValue>>
		{
			return new KeyValueDiffItemFactory<TType, TKey, TValue>();
		}

		public static OrderedDiffItemFactory<TValue[], TValue> Ordered<TValue>()
		{
			return DiffResultFactory.Ordered<TValue[], TValue>();
		}

		public static OrderedDiffItemFactory<TType, TValue> Ordered<TType, TValue>()
			where TType: IEnumerable<TValue>
		{
			return new OrderedDiffItemFactory<TType, TValue>();
		}

		public static UnorderedDiffItemFactory<HashSet<TValue>, TValue> Unordered<TValue>()
		{
			return DiffResultFactory.Unordered<HashSet<TValue>, TValue>();
		}

		public static UnorderedDiffItemFactory<TType, TValue> Unordered<TType, TValue>()
			where TType: IEnumerable<TValue>
		{
			return new UnorderedDiffItemFactory<TType, TValue>();
		}

		public static ValueDiffItemFactory<TType> Value<TType>()
		{
			return new ValueDiffItemFactory<TType>();
		}
	}
}
