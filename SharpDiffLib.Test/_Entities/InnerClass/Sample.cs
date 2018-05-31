namespace KST.SharpDiffLib.Test._Entities.InnerClass
{
	public class Sample
	{
		public string Value { get; set; }

		public string Value2 { get; set; }

		public SampleInner ValueInner { get; set; }

		public override string ToString()
		{
			return "<Sample>";
		}
	}
}
