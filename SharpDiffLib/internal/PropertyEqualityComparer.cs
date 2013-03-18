using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCOMerger.@internal
{
	internal class PropertyEqualityComparer<TType, TValueType> : IEqualityComparer<TType>
	{
		private readonly IEqualityComparer<TValueType> aValueEqualityComparer;
		private readonly Func<TType, TValueType> aGetValue;

		public PropertyEqualityComparer(Func<TType, TValueType> getValue, IEqualityComparer<TValueType> valueEqualityComparer)
		{
			this.aGetValue = getValue;
			this.aValueEqualityComparer = valueEqualityComparer;
		}

		public PropertyEqualityComparer(Func<TType, TValueType> getValue)
			: this (getValue, EqualityComparer<TValueType>.Default)
		{

		}

		#region Implementation of IEqualityComparer<in TType>

		public bool Equals(TType x, TType y)
		{
			return this.aValueEqualityComparer.Equals(this.aGetValue(x), this.aGetValue(y));
		}

		public int GetHashCode(TType obj)
		{
			return this.aValueEqualityComparer.GetHashCode(this.aGetValue(obj));
		}

		#endregion
	}
}
