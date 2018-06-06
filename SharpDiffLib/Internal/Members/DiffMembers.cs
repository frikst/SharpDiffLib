using System.Reflection;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.DiffResult.Base;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class DiffMembers
	{
		public static PropertyInfo Count()
		{
			return typeof(ICountableEnumerable<IDiffItem>).GetProperty(nameof(ICountableEnumerable<object>.Count));
		}
	}
}
