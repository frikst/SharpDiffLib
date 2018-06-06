using System.Reflection;
using KST.SharpDiffLib.ConflictManagement;

namespace KST.SharpDiffLib.Internal.Members
{
	internal static class ConflictContainerMembers
	{
		public static MethodInfo RegisterConflict()
		{
			return typeof(IConflictContainer).GetMethod(nameof(IConflictContainer.RegisterConflict));
		}
	}
}
