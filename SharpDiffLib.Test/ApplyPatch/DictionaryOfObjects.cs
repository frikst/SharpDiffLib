using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class DictionaryOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Dictionary<string, Sample>>()
					.ApplyKeyValueCollectionPatchRules();

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
			var diff = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Added("c", new Sample { Id = 3, Value = "c" })
				.MakeDiff();

			var obj = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneRemoved()
		{
			var diff = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Removed("c", new Sample { Id = 3, Value = "c" })
				.MakeDiff();

			var obj = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneReplaced()
		{
			var diff = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Replaced("c", new Sample { Id = 3, Value = "c" }, new Sample { Id = 4, Value = "d" })
				.MakeDiff();

			var obj = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 4, Value = "d" } }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneChanged()
		{
			var diff = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Changed("c", inner => inner.Class()
					.Replaced(x => x.Value, "c", "d")
				)
				.MakeDiff();

			var obj = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "d" } }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
