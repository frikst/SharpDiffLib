using System.Reflection;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class FastPropertyMembers
	{
		public static PropertyInfo UniqueID()
		{
			return typeof(Property).GetProperty(nameof(Property.UniqueID));
		}
	}
}
