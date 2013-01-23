using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.definition;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.ApplyOrderedCollectionPatchRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Added(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoAdded()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Added(2, 2)
				.Added(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2 };
			var changed = new[] { 1, 2, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void ManyAdded()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Added(0, 2)
				.Added(1, 2)
				.Added(2, 2)
				.Added(3, 2)
				.Added(4, 2)
				.MakeDiff();

			var obj = new[] { 1, 1, 1, 1 };
			var changed = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneRemoved()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoRemoved()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(2, 2)
				.Removed(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void ManyRemoved()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(0, 2)
				.Removed(1, 2)
				.Removed(2, 2)
				.Removed(3, 2)
				.Removed(4, 2)
				.MakeDiff();

			var obj = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 1, 1, 1, 1 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneReplaced()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(2, 3)
				.Added(2, 4)
				.MakeDiff();

			var obj = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneReplacedWithReplace()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Replaced(2, 3, 4)
				.MakeDiff();

			var obj = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoReplaced()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(2, 2)
				.Removed(2, 3)
				.Added(2, 4)
				.Added(2, 5)
				.MakeDiff();

			var obj = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2, 4, 5 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoReplacedWithReplace()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Replaced(2, 2, 4)
				.Replaced(3, 3, 5)
				.MakeDiff();

			var obj = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2, 4, 5 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void ManyReplaced()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(0, 2)
				.Added(0, 3)
				.Removed(1, 2)
				.Added(1, 3)
				.Removed(2, 2)
				.Added(2, 3)
				.Removed(3, 2)
				.Added(3, 3)
				.Removed(4, 2)
				.Added(4, 3)
				.MakeDiff();

			var obj = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 3, 1, 3, 1, 3, 1, 3, 1, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void ManyReplacedWithReplace()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Replaced(0, 2, 3)
				.Replaced(2, 2, 3)
				.Replaced(4, 2, 3)
				.Replaced(6, 2, 3)
				.Replaced(8, 2, 3)
				.MakeDiff();

			var obj = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 3, 1, 3, 1, 3, 1, 3, 1, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllReplaced()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(0, 2)
				.Removed(0, 2)
				.Removed(0, 2)
				.Removed(0, 2)
				.Removed(0, 2)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.MakeDiff();

			var obj = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllReplacedWithReplace()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Replaced(0, 2, 3)
				.Replaced(1, 2, 3)
				.Replaced(2, 2, 3)
				.Replaced(3, 2, 3)
				.Replaced(4, 2, 3)
				.MakeDiff();

			var obj = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllAdded()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.MakeDiff();

			var obj = new int[] { };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllRemoved()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.Removed(0, 2)
				.Removed(0, 2)
				.Removed(0, 2)
				.Removed(0, 2)
				.Removed(0, 2)
				.MakeDiff();

			var obj = new[] { 2, 2, 2, 2, 2 };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void EmptyUnchanged()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.MakeDiff();

			var obj = new int[] { };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void NonEmptyUnchanged()
		{
			IDiff<int[]> diff = DiffResultFactory<int[]>.Ordered<int>.Create()
				.MakeDiff();

			var obj = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 2, 2, 2, 2, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
