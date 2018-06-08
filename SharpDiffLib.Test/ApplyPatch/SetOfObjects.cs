using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class SetOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<Sample>>()
					.ApplyUnorderedCollectionPatchRules();

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
			var diff = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Added(new Sample { Id = 3, Value = "c" })
				.MakeDiff();

			var obj = new HashSet<Sample>
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

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x.Id).ToArray(), ret.ToArray());
		}

		[Test]
		public void OneRemoved()
		{
			var diff = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Removed(new Sample { Id = 3, Value = "c" })
				.MakeDiff();

			var obj = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x.Id).ToArray(), ret.ToArray());
		}

		[Test]
		public void OneReplaced()
		{
			var diff = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Replaced(new Sample { Id = 3, Value = "c" }, new Sample { Id = 4, Value = "d" })
				.MakeDiff();

			var obj = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 4, Value = "d" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x.Id).ToArray(), ret.ToArray());
		}

		[Test]
		public void OneChanged()
		{
			var diff = DiffFactory.Create<HashSet<Sample>>().Unordered()
				.Changed(3, inner => inner.Class()
					.Replaced(x => x.Value, "c", "d")
				)
				.MakeDiff();

			var obj = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};
			var changed = new HashSet<Sample>
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "d" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x.Id).ToArray(), ret.ToArray());
		}
	}
}
