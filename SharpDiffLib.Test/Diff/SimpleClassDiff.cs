using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class SimpleClassDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.ClassDiffRules();
			}
		}

		[Test]
		public void TestOneDifferent()
		{
			var @base = new Sample { Value = "one" };
			var changed = new Sample { Value = "two" };

			var expected = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value, "one", "two")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestAllDifferent()
		{
			var @base = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};
			var changed = new Sample
			{
				Value = "two",
				Value2 = "two2"
			};

			var expected = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value, "one", "two")
				.Replaced(x => x.Value2, "one2", "two2")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestNoneDifferent()
		{
			var @base = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};

			var expected = DiffResultFactory.Class<Sample>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestNullLeft()
		{
			var @base = new Sample
			{
				Value = "one",
				Value2 = null
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};

			var expected = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value2, null, "one2")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestNullRight()
		{
			var @base = new Sample
			{
				Value = "one",
				Value2 = "one2"
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = null
			};

			var expected = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value2, "one2", null)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestNullBoth()
		{
			var @base = new Sample
			{
				Value = "one",
				Value2 = null
			};
			var changed = new Sample
			{
				Value = "one",
				Value2 = null
			};

			var expected = DiffResultFactory.Class<Sample>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
