using System;
using System.Collections.Generic;
using System.Text;

namespace KST.SharpDiffLib.DiffResult.Factory
{
    public static class InnerDiffFactory
    {
	    public interface IParameter<TType, out THelperType>
	    {
		    
	    }

	    internal class Parameter<TType, THelperType> : IParameter<TType, THelperType>
	    {
			internal Parameter() { }
	    }

	    public static ClassDiffItemFactory<TType> Class<TType>(this IParameter<TType, TType> parameter)
	    {
		    return new ClassDiffItemFactory<TType>();
	    }

	    public static KeyValueDiffItemFactory<TType, TKey, TValue> KeyValue<TType, TKey, TValue>(this IParameter<TType, IEnumerable<KeyValuePair<TKey, TValue>>> parameter)
		    where TType : IEnumerable<KeyValuePair<TKey, TValue>>
	    {
		    return new KeyValueDiffItemFactory<TType, TKey, TValue>();
	    }

	    public static OrderedDiffItemFactory<TType, TValue> Ordered<TType, TValue>(this IParameter<TType, IEnumerable<TValue>> parameter)
		    where TType: IEnumerable<TValue>
	    {
		    return new OrderedDiffItemFactory<TType, TValue>();
	    }

	    public static UnorderedDiffItemFactory<TType, TValue> Unordered<TType, TValue>(this IParameter<TType, IEnumerable<TValue>> parameter)
		    where TType: IEnumerable<TValue>
	    {
		    return new UnorderedDiffItemFactory<TType, TValue>();
	    }

	    public static ValueDiffItemFactory<TType> Value<TType>(this IParameter<TType, TType> parameter)
	    {
		    return new ValueDiffItemFactory<TType>();
	    }
    }
}
