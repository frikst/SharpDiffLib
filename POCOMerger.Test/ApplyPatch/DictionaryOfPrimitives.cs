using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.definition;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class DictionaryOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Dictionary<string, int>>()
					.ApplyKeyValueCollectionPatchRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.Added("c", 3)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoAdded()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.Added("c", 3)
				.Added("d", 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoRemoved()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.Removed("c", 3)
				.Removed("d", 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneReplaced()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.Replaced("c", 3, 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 4 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllAdded()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.Added("a", 1)
				.Added("b", 2)
				.MakeDiff();

			var obj = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllRemoved()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.Removed("a", 1)
				.Removed("b", 2)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void UnchangedEmpty()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.MakeDiff();

			var obj = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void UnchangedNonEmpty()
		{
			IDiff<Dictionary<string, int>> diff = DiffResultFactory<Dictionary<string, int>>.KeyValue<string, int>.Create()
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
