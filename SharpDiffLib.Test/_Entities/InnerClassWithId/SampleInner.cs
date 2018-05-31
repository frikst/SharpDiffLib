namespace KST.SharpDiffLib.Test._Entities.InnerClassWithId
{
	public class SampleInner
	{
		public int Id { get; set; }

		public string Value { get; set; }

		public override string ToString()
		{
			return "<SampleInner:" + this.Id + ">";
		}

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return this.GetType() == obj.GetType()
				&& this.Value == ((SampleInner)obj).Value
				&& this.Id == ((SampleInner)obj).Id;
		}

		public override int GetHashCode()
		{
			return this.Id;
		}

		#endregion
	}
}