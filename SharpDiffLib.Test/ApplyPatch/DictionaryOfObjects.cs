using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.SimpleWithId;
using SharpDiffLib.algorithms.applyPatch;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult;

namespace SharpDiffLib.Test.ApplyPatch
{
	[TestClass]
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

		[TestMethod]
		public void OneAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, Sample>.Create()
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

		[TestMethod]
		public void OneRemoved()
		{
			var diff = DiffResultFactory.KeyValue<string, Sample>.Create()
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

		[TestMethod]
		public void OneReplaced()
		{
			var diff = DiffResultFactory.KeyValue<string, Sample>.Create()
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

		[TestMethod]
		public void OneChanged()
		{
			var diff = DiffResultFactory.KeyValue<string, Sample>.Create()
				.Changed("c", DiffResultFactory<Sample>.Class.Create()
					.Replaced(x => x.Value, "c", "d")
					.MakeDiff()
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
