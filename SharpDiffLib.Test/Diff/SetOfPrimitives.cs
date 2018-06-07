using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class SetOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<int>>()
					.UnorderedCollectionDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoAdded()
		{
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3, 4 };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.Added(4)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoRemoved()
		{
			var @base = new HashSet<int> { 1, 2, 3, 4 };
			var changed = new HashSet<int> { 1, 2 };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.Removed(3)
				.Removed(4)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllAdded()
		{
			var @base = new HashSet<int> { };
			var changed = new HashSet<int> { 1, 2 };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.Added(1)
				.Added(2)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllRemoved()
		{
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.Removed(1)
				.Removed(2)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void UnchangedEmpty()
		{
			var @base = new HashSet<int> { };
			var changed = new HashSet<int> { };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void UnchangedNonEmpty()
		{
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2 };

			var expected = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
