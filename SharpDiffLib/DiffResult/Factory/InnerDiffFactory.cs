using System;
using System.Collections.Generic;
using System.Text;

namespace KST.SharpDiffLib.DiffResult.Factory
{
    public static class InnerDiffFactory
    {
	    public interface IDiffFactory<TType, out THelperType>
	    {
		    
	    }

	    public static ClassDiffItemFactory<TType> Class<TType>(this IDiffFactory<TType, TType> diffFactory)
	    {
		    return new ClassDiffItemFactory<TType>();
	    }

	    public static KeyValueDiffItemFactory<TType, TKey, TValue> KeyValue<TType, TKey, TValue>(this IDiffFactory<TType, IEnumerable<KeyValuePair<TKey, TValue>>> diffFactory)
		    where TType : IEnumerable<KeyValuePair<TKey, TValue>>
	    {
		    return new KeyValueDiffItemFactory<TType, TKey, TValue>();
	    }

	    public static OrderedDiffItemFactory<TType, TValue> Ordered<TType, TValue>(this IDiffFactory<TType, IEnumerable<TValue>> diffFactory)
		    where TType: IEnumerable<TValue>
	    {
		    return new OrderedDiffItemFactory<TType, TValue>();
	    }

	    public static UnorderedDiffItemFactory<TType, TValue> Unordered<TType, TValue>(this IDiffFactory<TType, IEnumerable<TValue>> diffFactory)
		    where TType: IEnumerable<TValue>
	    {
		    return new UnorderedDiffItemFactory<TType, TValue>();
	    }

	    public static ValueDiffItemFactory<TType> Value<TType>(this IDiffFactory<TType, TType> diffFactory)
	    {
		    return new ValueDiffItemFactory<TType>();
	    }
    }
}
