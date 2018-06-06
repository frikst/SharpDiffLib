using System.Reflection;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class ObjectMembers
	{
		public static MethodInfo Equals()
		{
			return typeof(object).GetMethod(nameof(object.Equals), BindingFlags.Static | BindingFlags.Public);
		}
	}
}
