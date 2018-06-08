using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.InnerClassWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class StructuredClassDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleInner>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.ClassDiffRules();

				Define<Sample>()
					.ClassDiffRules();
			}
		}

		[Test]
		public void TestInnerChanged()
		{
			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "two"
				}
			};

			var expected = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.ValueInner, inner => inner.Class()
					.Replaced(x => x.Value, "one", "two")
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestInnerSame()
		{
			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};

			var expected = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestInnerReplaced()
		{
			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 2,
					Value = "one"
				}
			};

			var expected = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.ValueInner, new SampleInner {Id = 1, Value = "one"}, new SampleInner {Id = 2, Value = "one"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestInnerNullLeft()
		{
			var @base = new Sample
			{
				Value = "one",
				ValueInner = null
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 2,
					Value = "one"
				}
			};

			var expected = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.ValueInner, null, new SampleInner {Id = 2, Value = "one"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestInnerNullRight()
		{
			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = null
			};

			var expected = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.ValueInner, new SampleInner {Id = 1, Value = "one"}, null)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestInnerNullBoth()
		{
			var @base = new Sample
			{
				Value = "one",
				ValueInner = null
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = null
			};

			var expected = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
