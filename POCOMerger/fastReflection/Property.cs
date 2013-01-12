using System.Reflection;

namespace POCOMerger.fastReflection
{
	public class Property
	{
		private static int aLastID = 0;

		public Property(PropertyInfo reflectionPropertyInfo)
		{
			this.Name = reflectionPropertyInfo.Name;
			this.UniqueID = aLastID++;
			this.ReflectionPropertyInfo = reflectionPropertyInfo;
		}

		public string Name { get; private set; }
		public int UniqueID { get; private set; }
		public PropertyInfo ReflectionPropertyInfo { get; private set; }
	}
}