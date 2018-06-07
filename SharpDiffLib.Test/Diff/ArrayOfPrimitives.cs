using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<int[]>()
					.OrderedCollectionDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var expected = DiffResultFactory.Ordered<int>()
				.Added(2, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoAdded()
		{
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 2, 3 };

			var expected = DiffResultFactory.Ordered<int>()
				.Added(2, 2)
				.Added(2, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void ManyAdded()
		{
			var @base = new[] { 1, 1, 1, 1 };
			var changed = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

			var expected = DiffResultFactory.Ordered<int>()
				.Added(0, 2)
				.Added(1, 2)
				.Added(2, 2)
				.Added(3, 2)
				.Added(4, 2)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneRemoved()
		{
			var @base = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2 };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(2, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoRemoved()
		{
			var @base = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2 };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(2, 2)
				.Removed(3, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void ManyRemoved()
		{
			var @base = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 1, 1, 1, 1 };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(0, 2)
				.Removed(2, 2)
				.Removed(4, 2)
				.Removed(6, 2)
				.Removed(8, 2)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneReplaced()
		{
			var @base = new[] { 1, 2, 3 };
			var changed = new[] { 1, 2, 4 };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(2, 3)
				.Added(3, 4)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoReplaced()
		{
			var @base = new[] { 1, 2, 2, 3 };
			var changed = new[] { 1, 2, 4, 5 };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(2, 2)
				.Removed(3, 3)
				.Added(4, 4)
				.Added(4, 5)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void ManyReplaced()
		{
			var @base = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
			var changed = new[] { 3, 1, 3, 1, 3, 1, 3, 1, 3 };

			var expected = DiffResultFactory.Ordered<int>()
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

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllReplaced()
		{
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(0, 2)
				.Removed(1, 2)
				.Removed(2, 2)
				.Removed(3, 2)
				.Removed(4, 2)
				.Added(5, 3)
				.Added(5, 3)
				.Added(5, 3)
				.Added(5, 3)
				.Added(5, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllAdded()
		{
			var @base = new int[] { };
			var changed = new[] { 3, 3, 3, 3, 3 };

			var expected = DiffResultFactory.Ordered<int>()
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.Added(0, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllRemoved()
		{
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new int[] { };

			var expected = DiffResultFactory.Ordered<int>()
				.Removed(0, 2)
				.Removed(1, 2)
				.Removed(2, 2)
				.Removed(3, 2)
				.Removed(4, 2)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void EmptyUnchanged()
		{
			var @base = new int[] { };
			var changed = new int[] { };

			var expected = DiffResultFactory.Ordered<int>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void NonEmptyUnchanged()
		{
			var @base = new[] { 2, 2, 2, 2, 2 };
			var changed = new[] { 2, 2, 2, 2, 2 };

			var expected = DiffResultFactory.Ordered<int>()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
