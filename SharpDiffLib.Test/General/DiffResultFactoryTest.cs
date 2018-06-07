using System.Text;
using KST.SharpDiffLib.DiffResult.Factory;
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
			var diff = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(x => x.Value, "Value", "Value2")
				.Unchanged(x => x.Value2, "xxx")
				.MakeDiff();

			var expected = new StringBuilder();
			expected.AppendLine("=ValueInner:");
			expected.AppendLine("\t-Value:Hello");
			expected.AppendLine("\t+Value:World");
			expected.AppendLine("-Value:Value");
			expected.AppendLine("+Value:Value2");
			expected.Append(" Value2:xxx");

			Assert.AreEqual(expected.ToString(), diff.ToString());
		}

		[Test]
		public void Array()
		{
			var diff = DiffResultFactory.Ordered<SampleInner>()
				.Changed(0, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(1, new SampleInner(), new SampleInner())
				.Removed(2, new SampleInner())
				.Added(2, new SampleInner())
				.Unchanged(3, new SampleInner())
				.MakeDiff();

			var expected = new StringBuilder();
			expected.AppendLine("=0:");
			expected.AppendLine("\t-Value:Hello");
			expected.AppendLine("\t+Value:World");
			expected.AppendLine("-1:<SampleInner>");
			expected.AppendLine("+1:<SampleInner>");
			expected.AppendLine("-2:<SampleInner>");
			expected.AppendLine("+2:<SampleInner>");
			expected.Append(" 3:<SampleInner>");

			Assert.AreEqual(expected.ToString(), diff.ToString());
		}

		[Test]
		public void Dictionary()
		{
			var diff = DiffResultFactory.KeyValue<string, SampleInner>()
				.Changed("a", DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced("b", new SampleInner(), new SampleInner())
				.Removed("c", new SampleInner())
				.Added("d", new SampleInner())
				.Unchanged("e", new SampleInner())
				.MakeDiff();

			var expected = new StringBuilder();
			expected.AppendLine("=a:");
			expected.AppendLine("\t-Value:Hello");
			expected.AppendLine("\t+Value:World");
			expected.AppendLine("-b:<SampleInner>");
			expected.AppendLine("+b:<SampleInner>");
			expected.AppendLine("-c:<SampleInner>");
			expected.AppendLine("+d:<SampleInner>");
			expected.Append(" e:<SampleInner>");

			Assert.AreEqual(expected.ToString(), diff.ToString());
		}

		[Test]
		public void Set()
		{
			var diff = DiffResultFactory.Unordered<SampleInner>()
				.Changed(1, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "Hello", "World")
					.MakeDiff()
				)
				.Replaced(new SampleInner(), new SampleInner())
				.Removed(new SampleInner())
				.Added(new SampleInner())
				.Unchanged(new SampleInner())
				.MakeDiff();

			var expected = new StringBuilder();
			expected.AppendLine("=1:");
			expected.AppendLine("\t-Value:Hello");
			expected.AppendLine("\t+Value:World");
			expected.AppendLine("-<SampleInner>");
			expected.AppendLine("+<SampleInner>");
			expected.AppendLine("-<SampleInner>");
			expected.AppendLine("+<SampleInner>");
			expected.Append(" <SampleInner>");

			Assert.AreEqual(expected.ToString(), diff.ToString());
		}

		[Test]
		public void Conflicts()
		{
			var diff = DiffResultFactory.Class<SampleInner>()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			var expected = new StringBuilder();
			expected.AppendLine("C<<<");
			expected.AppendLine("\t-Value:a");
			expected.AppendLine("\t+Value:b");
			expected.AppendLine("C>>>");
			expected.AppendLine("\t-Value:a");
			expected.Append("\t+Value:c");

			Assert.AreEqual(expected.ToString(), diff.ToString());
		}
	}
}
