using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharpDiffLib.@internal
{
	public class IdentityComparer<T> : IEqualityComparer<T>
		where T : class
	{
		public bool Equals(T left, T right)
		{
			return (object)left == (object)right;
		}

		public int GetHashCode(T value)
		{
			return RuntimeHelpers.GetHashCode(value);
		}
	}
}
