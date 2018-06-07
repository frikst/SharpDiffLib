using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.BaseWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class BaseDefinition
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.BaseClassDiffRules()
					.Inheritable.ClassDiffRules()
					.Inheritable.GeneralRules(rules => rules
						.Id(x => x.Id)
					);

				Define<SampleBase[]>()
					.OrderedCollectionDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var @base = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" }
			};
			var changed = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};

			var expected = DiffResultFactory.Ordered<SampleBase>()
				.Added(1, new SampleDescendant1 {Id = 2, Value = "b"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneReplacedWithOtherDescendant()
		{
			var @base = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant2 { Id = 2, Value = "b" }
			};

			var expected = DiffResultFactory.Ordered<SampleBase>()
				.Changed(1, DiffResultFactory.Value<SampleBase>()
					.Replaced(new SampleDescendant1 {Id = 2, Value = "b"}, new SampleDescendant2 {Id = 2, Value = "b"})
					.MakeDiff()
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneChangedProperty()
		{
			var @base = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "c" }
			};

			var expected = DiffResultFactory.Ordered<SampleBase>()
				.Changed(1, DiffResultFactory.Value<SampleBase>()
					.Changed(DiffResultFactory.Class<SampleDescendant1>()
						.Replaced(x => x.Value, "b", "c")
						.MakeDiff()
					)
					.MakeDiff()
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
