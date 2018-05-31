using System;
using System.Text;

namespace KST.SharpDiffLib.Internal
{
	internal static class StringBuilderExtensions
	{
		public static StringBuilder AppendIndent(this StringBuilder sb, int indentationLevel)
		{
			return sb.Append(new String('\t', indentationLevel));
		}

		public static StringBuilder AppendNullable(this StringBuilder sb, object value)
		{
			if (object.ReferenceEquals(value, null))
				return sb.Append("(null)");
			else
				return sb.Append(value);
		}
	}
}
