using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class ArrayOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample[]>()
					.OrderedCollectionDiffRules();

				Define<Sample>()
					.ClassDiffRules()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					);
			}
		}

		[Test]
		public void OneAdded()
		{
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};

			var expected = DiffResultFactory.Ordered<Sample>()
				.Added(2, new Sample {Id = 3, Value = "c"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneRemoved()
		{
			var @base = new[]
			{
				new Sample { Id = 0, Value = "a" },
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};

			var expected = DiffResultFactory.Ordered<Sample>()
				.Removed(0, new Sample {Id = 0, Value = "a"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneChanged()
		{
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "c" },
				new Sample { Id = 2, Value = "b" }
			};

			var expected = DiffResultFactory.Ordered<Sample>()
				.Changed(0, inner => inner.Class()
					.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneReplacedWithNull()
		{
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				null
			};

			var expected = DiffResultFactory.Ordered<Sample>()
				.Removed(1, new Sample {Id = 2, Value = "b"})
				.Added(2, null)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
