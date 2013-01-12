using System.Collections.Generic;
using System.Linq;

namespace POCOMerger.fastReflection
{
	public static class Class<TClass>
	{
		private static IEnumerable<Property> aProperties = null;

		public static IEnumerable<Property> Properties
		{
			get
			{
				if (aProperties == null)
					aProperties = typeof(TClass).GetProperties().Select(x => new Property(x)).ToList().AsReadOnly();

				return aProperties;
			}
		}
	}
}
