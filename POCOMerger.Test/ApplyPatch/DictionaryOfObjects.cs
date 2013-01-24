﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diffResult;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class DictionaryOfObjects
	{
		private class Sample
		{
			public int Id { get; set; }

			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample:" + this.Id + ">";
			}

			#region Equality members

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj))
					return false;

				if (ReferenceEquals(this, obj))
					return true;

				if (!(obj is Sample))
					return false;

				return this.Id == ((Sample)obj).Id && this.Value == ((Sample)obj).Value;
			}

			public override int GetHashCode()
			{
				return this.Id;
			}

			#endregion
		}

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