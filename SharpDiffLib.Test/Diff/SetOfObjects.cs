using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class SetOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<Sample>>()
					.UnorderedCollectionDiffRules();
				Define<Sample>()
					.GeneralRules(rules => rules
					    .Id(x => x.Id)
					)
					.ClassDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var @base = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" }, 
				new Sample { Id = 2, Value = "b" }, 
				new Sample { Id = 3, Value = "c" }
			};

			var expected = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Added(new Sample {Id = 3, Value = "c"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoAdded()
		{
			var @base = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" },
				new Sample { Id = 4, Value = "d" }
			};

			var expected = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Added(new Sample {Id = 3, Value = "c"})
				.Added(new Sample {Id = 4, Value = "d"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoRemoved()
		{
			var @base = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" },
				new Sample { Id = 4, Value = "d" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};

			var expected = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Removed(new Sample {Id = 3, Value = "c"})
				.Removed(new Sample {Id = 4, Value = "d"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneChanged()
		{
			var @base = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "c" },
				new Sample { Id = 2, Value = "b" }
			};

			var expected = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Changed(1, inner => inner.Class()
					.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
