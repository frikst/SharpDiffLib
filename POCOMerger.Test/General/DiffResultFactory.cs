using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult;

namespace POCOMerger.Test.General
{
	[TestClass]
	public class DiffResultFactory
	{
		private class SampleInner
		{
			public string Value { get; set; }

			public override string ToString()
			{
				return "<SampleInner>";
			}
		}

		private class Sample
		{
			public string Value { get; set; }

			public SampleInner Inner { get; set; }

			public override string ToString()
			{
				return "<Sample>";
			}
		}

		[TestMethod]
		public void Class()
		{
			IDiff diff = DiffResultFactory<Sample>.Class.Create()
				.Changed(x => x.Inner, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(x => x.Value, "Value", "Value2")
				.MakeDiff();

			const string diffString =
				"=Inner:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-Value:Value\r\n" +
				"+Value:Value2";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Array()
		{
			IDiff diff = DiffResultFactory<SampleInner[]>.Ordered<SampleInner>.Create()
				.Changed(0, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(1, new SampleInner(), new SampleInner())
				.Removed(2, new SampleInner())
				.Added(2, new SampleInner())
				.MakeDiff();

			const string diffString =
				"=0:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-1:<SampleInner>\r\n" +
				"+1:<SampleInner>\r\n" +
				"-2:<SampleInner>\r\n" +
				"+2:<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Dictionary()
		{
			IDiff diff = DiffResultFactory<Dictionary<string, SampleInner>>.KeyValue<string, SampleInner>.Create()
				.Changed("a", DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced("b", new SampleInner(), new SampleInner())
				.Removed("c", new SampleInner())
				.Added("d", new SampleInner())
				.MakeDiff();

			const string diffString =
				"=a:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-b:<SampleInner>\r\n" +
				"+b:<SampleInner>\r\n" +
				"-c:<SampleInner>\r\n" +
				"+d:<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[TestMethod]
		public void Set()
		{
			IDiff diff = DiffResultFactory<HashSet<SampleInner>>.Unordered<SampleInner>.Create()
				.Changed(1, DiffResultFactory<SampleInner>.Class.Create()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(new SampleInner(), new SampleInner())
				.Removed(new SampleInner())
				.Added(new SampleInner())
				.MakeDiff();

			const string diffString =
				"=1:\r\n" +
				"\t-Value:Hello\r\n" +
				"\t+Value:World\r\n" +
				"-<SampleInner>\r\n" +
				"+<SampleInner>\r\n" +
				"-<SampleInner>\r\n" +
				"+<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}
	}
}
