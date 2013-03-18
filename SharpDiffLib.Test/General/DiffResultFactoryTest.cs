using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.InnerClass;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.General
{
	[TestClass]
	public class DiffResultFactoryTest
	{
		[TestMethod]
		public void Class()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(x => x.Value, "Value", "Value2")
				.Unchanged(x => x.Value2, "xxx")
				.MakeDiff();

			const string diffString =
				"=ValueInner:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-Value:Value\r\n" +
				"+Value:Value2\r\n" +
				" Value2:xxx";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Array()
		{
			var diff = DiffResultFactory.Ordered<SampleInner>.Create()
				.Changed(0, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(1, new SampleInner(), new SampleInner())
				.Removed(2, new SampleInner())
				.Added(2, new SampleInner())
				.Unchanged(3, new SampleInner())
				.MakeDiff();

			const string diffString =
				"=0:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-1:<SampleInner>\r\n" +
				"+1:<SampleInner>\r\n" +
				"-2:<SampleInner>\r\n" +
				"+2:<SampleInner>\r\n" +
				" 3:<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Dictionary()
		{
			var diff = DiffResultFactory.KeyValue<string, SampleInner>.Create()
				.Changed("a", DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced("b", new SampleInner(), new SampleInner())
				.Removed("c", new SampleInner())
				.Added("d", new SampleInner())
				.Unchanged("e", new SampleInner())
				.MakeDiff();

			const string diffString =
				"=a:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-b:<SampleInner>\r\n" +
				"+b:<SampleInner>\r\n" +
				"-c:<SampleInner>\r\n" +
				"+d:<SampleInner>\r\n" +
				" e:<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Set()
		{
			var diff = DiffResultFactory.Unordered<SampleInner>.Create()
				.Changed(1, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(new SampleInner(), new SampleInner())
				.Removed(new SampleInner())
				.Added(new SampleInner())
				.Unchanged(new SampleInner())
				.MakeDiff();

			const string diffString =
				"=1:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-<SampleInner>\r\n" +
				"+<SampleInner>\r\n" +
				"-<SampleInner>\r\n" +
				"+<SampleInner>\r\n" +
				" <SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Conflicts()
		{
			var diff = DiffResultFactory.Class<SampleInner>.Create()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			const string diffString =
				"C<<<\r\n" +
				"\t-Value:a\r\n" +
				"\t+Value:b\r\n" +
				"C>>>\r\n" +
				"\t-Value:a\r\n" +
				"\t+Value:c";

			Assert.AreEqual(diffString, diff.ToString());
		}
	}
}
