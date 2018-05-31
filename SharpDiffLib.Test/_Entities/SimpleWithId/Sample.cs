namespace KST.SharpDiffLib.Test._Entities.SimpleWithId
{
	public class Sample
	{
		public int Id { get; set; }

		public string Value { get; set; }

		public override string ToString()
		{
			return "<Sample:" + this.Id + ">";
		}

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return this.GetType() == obj.GetType()
				&& this.Value == ((Sample)obj).Value
				&& this.Id == ((Sample)obj).Id;
		}

		public override int GetHashCode()
		{
			return this.Id;
		}

		#endregion
	}

}
