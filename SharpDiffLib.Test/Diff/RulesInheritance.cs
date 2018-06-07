using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.BaseWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class RulesInheritance
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.Inheritable.ClassDiffRules()
					.Inheritable.GeneralRules(rules => rules
						.Id(x => x.Id)
					);

				Define<SampleDescendant1[]>()
					.OrderedCollectionDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var @base = new[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 3, Value = "c" }
			};
			var changed = new[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" },
				new SampleDescendant1 { Id = 3, Value = "c" }
			};

			var expected = DiffResultFactory.Ordered<SampleDescendant1>()
				.Added(1, new SampleDescendant1 {Id = 2, Value = "b"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneChanged()
		{
			var @base = new[]
			{
				new SampleDescendant1 { Id = 0, Value = "a" },
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new SampleDescendant1 { Id = 0, Value = "b" },
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};

			var expected = DiffResultFactory.Ordered<SampleDescendant1>()
				.Changed(0, DiffResultFactory.Class<SampleDescendant1>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff())
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
