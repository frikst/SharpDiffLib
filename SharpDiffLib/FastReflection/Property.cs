using System;
using System.Reflection;

namespace KST.SharpDiffLib.FastReflection
{
	public class Property
	{
		private static int aLastID = 0;

		public Property(PropertyInfo reflectionPropertyInfo)
		{
			this.Name = reflectionPropertyInfo.Name;
			this.UniqueID = aLastID++;
			this.ReflectionPropertyInfo = reflectionPropertyInfo;
			this.Type = reflectionPropertyInfo.PropertyType;
		}

		public string Name { get; }
		public Type Type { get; }

		public int UniqueID { get; }

		public PropertyInfo ReflectionPropertyInfo { get; }
	}
}