namespace KST.SharpDiffLib.Test._Entities.BaseWithId
{
	public class SampleDescendant1 : SampleBase
	{
		public string Value { get; set; }

		public override string ToString()
		{
			return "<Sample1:" + this.Id + ">";
		}

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return this.GetType() == obj.GetType()
				&& this.Value == ((SampleDescendant1)obj).Value
				&& this.Id == ((SampleDescendant1)obj).Id;
		}

		public override int GetHashCode()
		{
			return this.Id;
		}

		#endregion
	}
}