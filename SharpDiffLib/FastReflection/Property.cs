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

		public string Name { get; private set; }
		public Type Type { get; private set; }

		public int UniqueID { get; private set; }

		public PropertyInfo ReflectionPropertyInfo { get; private set; }
	}
}