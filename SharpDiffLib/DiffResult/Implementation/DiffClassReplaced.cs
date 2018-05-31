using System;
using System.Text;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;
using KST.SharpDiffLib.Internal;

namespace KST.SharpDiffLib.DiffResult.Implementation
{
	internal class DiffClassReplaced<TItemType> : IDiffItemReplaced<TItemType>, IDiffClassItem
	{
		public DiffClassReplaced(Property property, TItemType oldValue, TItemType newValue)
		{
			this.Property = property;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		#region Implementation of IDiffItem

		public bool IsSame(IDiffItem other)
		{
			if (!(other is IDiffItemReplaced<TItemType>))
				return false;

			return object.Equals(this.OldValue, ((IDiffItemReplaced<TItemType>) other).OldValue)
				&& object.Equals(this.NewValue, ((IDiffItemReplaced<TItemType>) other).NewValue);
		}

		public System.Type ItemType
		{
			get { return typeof(TItemType); }
		}

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			ret.AppendIndent(indentLevel).Append('-').Append(this.Property.Name).Append(':').AppendNullable(this.OldValue).AppendLine();
			ret.AppendIndent(indentLevel).Append('+').Append(this.Property.Name).Append(':').AppendNullable(this.NewValue);

			return ret.ToString();
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		#endregion

		#region Implementation of IDiffItemReplaced

		object IDiffItemReplaced.OldValue
		{
			get { return this.OldValue; }
		}

		object IDiffItemReplaced.NewValue
		{
			get { return this.NewValue; }
		}

		#endregion

		#region Implementation of IDiffItemReplaced<TItemType>

		public TItemType NewValue { get; private set; }

		public TItemType OldValue { get; private set; }

		#endregion

		#region Implementation of IDiffClassItem

		public Property Property { get; private set; }

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiffClassItem && obj is IDiffItemReplaced<TItemType>))
				return false;

			return object.Equals(this.OldValue, ((IDiffItemReplaced<TItemType>)obj).OldValue)
				&& object.Equals(this.NewValue, ((IDiffItemReplaced<TItemType>)obj).NewValue)
				&& object.Equals(this.Property, ((IDiffClassItem)obj).Property);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion
	}
}
