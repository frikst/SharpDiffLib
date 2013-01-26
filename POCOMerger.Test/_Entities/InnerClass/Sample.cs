namespace POCOMerger.Test._Entities.InnerClass
{
	public class Sample
	{
		public string Value { get; set; }

		public SampleInner ValueInner { get; set; }

		public override string ToString()
		{
			return "<Sample>";
		}
	}
}
