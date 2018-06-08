using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class ArrayOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample[]>()
					.ApplyOrderedCollectionPatchRules();

				Define<Sample>()
					.ApplyClassPatchRules()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					);
			}
		}

		[Test]
		public void OneAdded()
		{
			var diff = DiffResultFactory.Ordered<Sample>()
				.Added(2, new Sample { Id = 3, Value = "c" })
				.MakeDiff();
			
			var obj = new[]
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

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneRemoved()
		{
			var diff = DiffResultFactory.Ordered<Sample>()
				.Removed(2, new Sample { Id = 3, Value = "c" })
				.MakeDiff();

			var obj = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneReplaced()
		{
			var diff = DiffResultFactory.Ordered<Sample>()
				.Replaced(2, new Sample { Id = 3, Value = "c" }, new Sample { Id = 4, Value = "d" })
				.MakeDiff();

			var obj = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 4, Value = "d" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneChanged()
		{
			var diff = DiffResultFactory.Ordered<Sample>()
				.Changed(2, inner => inner.Class()
					.Replaced(x => x.Value, "c", "d")
				)
				.MakeDiff();

			var obj = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "d" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TwoChanged()
		{
			var diff = DiffResultFactory.Ordered<Sample>()
				.Changed(1, inner => inner.Class()
					.Replaced(x => x.Value, "b", "d")
				)
				.Changed(2, inner => inner.Class()
					.Replaced(x => x.Value, "c", "d")
				)
				.MakeDiff();

			var obj = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "d" },
				new Sample { Id = 3, Value = "d" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
