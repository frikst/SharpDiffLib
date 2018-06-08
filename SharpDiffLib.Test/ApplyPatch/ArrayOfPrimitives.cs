using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
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

		[Test]
		public void OneAdded()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Added(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TwoAdded()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Added(2, 2)
				.Added(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2 };
			var changed = new[] { 1, 2, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void ManyAdded()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void OneRemoved()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Removed(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TwoRemoved()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Removed(2, 2)
				.Removed(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void ManyRemoved()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Removed(0, 2)
				.Removed(2, 2)
				.Removed(4, 2)
				.Removed(6, 2)
				.Removed(8, 2)
				.MakeDiff();

			var obj = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 1, 1, 1, 1 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneReplaced()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Removed(2, 3)
				.Added(2, 4)
				.MakeDiff();

			var obj = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneReplacedWithReplace()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Replaced(2, 3, 4)
				.MakeDiff();

			var obj = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TwoReplaced()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void TwoReplacedWithReplace()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Replaced(2, 2, 4)
				.Replaced(3, 3, 5)
				.MakeDiff();

			var obj = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2, 4, 5 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void ManyReplaced()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.Removed(0, 2)
				.Added(1, 3)
				.Removed(2, 2)
				.Added(3, 3)
				.Removed(4, 2)
				.Added(5, 3)
				.Removed(6, 2)
				.Added(7, 3)
				.Removed(8, 2)
				.Added(9, 3)
				.MakeDiff();

			var obj = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 3, 1, 3, 1, 3, 1, 3, 1, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void ManyReplacedWithReplace()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void AllReplaced()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void AllReplacedWithReplace()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void AllAdded()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void AllRemoved()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
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

		[Test]
		public void EmptyUnchanged()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();

			var obj = new int[] { };
			var changed = new int[] { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void NonEmptyUnchanged()
		{
			var diff = DiffFactory.Create<int[]>().Ordered()
				.MakeDiff();

			var obj = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 2, 2, 2, 2, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
