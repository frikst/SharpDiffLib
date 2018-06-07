using System;
using KST.SharpDiffLib.DiffResult;
using KST.SharpDiffLib.Test._Entities.InnerClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.General
{
	[TestFixture]
	public class DiffResultFactoryTest
	{
		[Test]
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

			string diffString =
				"=ValueInner:" + Environment.NewLine +
				"\t-Value:Hello" + Environment.NewLine +
				"\t+Value:World" + Environment.NewLine +
				"-Value:Value" + Environment.NewLine +
				"+Value:Value2" + Environment.NewLine +
				" Value2:xxx";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[Test]
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

			string diffString =
				"=0:" + Environment.NewLine +
				"\t-Value:Hello" + Environment.NewLine +
				"\t+Value:World" + Environment.NewLine +
				"-1:<SampleInner>" + Environment.NewLine +
				"+1:<SampleInner>" + Environment.NewLine +
				"-2:<SampleInner>" + Environment.NewLine +
				"+2:<SampleInner>" + Environment.NewLine +
				" 3:<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[Test]
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

			string diffString =
				"=a:" + Environment.NewLine +
				"\t-Value:Hello" + Environment.NewLine +
				"\t+Value:World" + Environment.NewLine +
				"-b:<SampleInner>" + Environment.NewLine +
				"+b:<SampleInner>" + Environment.NewLine +
				"-c:<SampleInner>" + Environment.NewLine +
				"+d:<SampleInner>" + Environment.NewLine +
				" e:<SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[Test]
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

			string diffString =
				"=1:" + Environment.NewLine +
				"\t-Value:Hello" + Environment.NewLine +
				"\t+Value:World" + Environment.NewLine +
				"-<SampleInner>" + Environment.NewLine +
				"+<SampleInner>" + Environment.NewLine +
				"-<SampleInner>" + Environment.NewLine +
				"+<SampleInner>" + Environment.NewLine +
				" <SampleInner>";

			Assert.AreEqual(diffString, diff.ToString());
		}

		[Test]
		public void Conflicts()
		{
			var diff = DiffResultFactory.Class<SampleInner>.Create()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			string diffString =
				"C<<<" + Environment.NewLine +
				"\t-Value:a" + Environment.NewLine +
				"\t+Value:b" + Environment.NewLine +
				"C>>>" + Environment.NewLine +
				"\t-Value:a" + Environment.NewLine +
				"\t+Value:c";

			Assert.AreEqual(diffString, diff.ToString());
		}
	}
}
