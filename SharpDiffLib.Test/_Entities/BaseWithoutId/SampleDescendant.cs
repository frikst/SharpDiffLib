namespace KST.SharpDiffLib.Test._Entities.BaseWithoutId
{
	public class SampleDescendant : SampleBase
	{
		public string Value3 { get; set; }

		public string Value4 { get; set; }

		public override string ToString()
		{
			return "<Sample>";
		}
	}
}