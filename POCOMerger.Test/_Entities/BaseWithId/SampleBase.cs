namespace POCOMerger.Test._Entities.BaseWithId
{
	public abstract class SampleBase
	{
		public int Id { get; set; }

		public string ValueBase { get; set; }

		public override string ToString()
		{
			return "<SampleBase:" + this.Id + ">";
		}

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			return this.GetType() == obj.GetType()
				&& this.Id == ((SampleBase)obj).Id;
		}

		public override int GetHashCode()
		{
			return this.Id;
		}

		#endregion
	}
}
