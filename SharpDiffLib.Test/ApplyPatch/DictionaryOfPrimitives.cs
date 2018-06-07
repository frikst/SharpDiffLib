using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
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

		[Test]
		public void OneAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.Added("c", 3)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TwoAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.Added("c", 3)
				.Added("d", 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TwoRemoved()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.Removed("c", 3)
				.Removed("d", 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneReplaced()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.Replaced("c", 3, 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 4 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void AllAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.Added("a", 1)
				.Added("b", 2)
				.MakeDiff();

			var obj = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void AllRemoved()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.Removed("a", 1)
				.Removed("b", 2)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void UnchangedEmpty()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.MakeDiff();

			var obj = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void UnchangedNonEmpty()
		{
			var diff = DiffResultFactory.KeyValue<string, int>()
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
