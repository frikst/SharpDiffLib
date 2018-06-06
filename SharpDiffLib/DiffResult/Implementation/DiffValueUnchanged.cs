using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffValueUnchanged<TItemType> : IDiffItemUnchanged<TItemType>, IDiffValue
	{
		public DiffValueUnchanged(TItemType value)
		{
			this.Value = value;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemUnchanged<TItemType>))
				return false;

			return object.Equals(this.Value, ((IDiffItemUnchanged<TItemType>)other).Value);
		}

		public System.Type ItemType
			=> typeof(TItemType);

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('-').AppendNullable(this.Value);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemUnchanged

		object IDiffItemUnchanged.Value
			=> this.Value;

		#endregion

		#region Implementation of IDiffItemUnchanged<TItemType>

		public TItemType Value { get; private set; }

		#endregion

		#region Implementation of IDiffValue

		public System.Type ValueType
			=> typeof(TItemType);

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffValue && obj is IDiffItemUnchanged<TItemType>))
				return false;

			return object.Equals(this.Value, ((IDiffItemUnchanged<TItemType>)obj).Value);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
