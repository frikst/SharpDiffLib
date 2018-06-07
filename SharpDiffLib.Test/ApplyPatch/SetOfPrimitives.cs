using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class SetOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<int>>()
					.ApplyUnorderedCollectionPatchRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.Added(3)
			    .MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void TwoAdded()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.Added(3)
				.Added(4)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3, 4 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void TwoRemoved()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.Removed(3)
				.Removed(4)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2, 3, 4 };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void AllAdded()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.Added(1)
				.Added(2)
				.MakeDiff();

			var obj = new HashSet<int> { };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void AllRemoved()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.Removed(1)
				.Removed(2)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void UnchangedEmpty()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.MakeDiff();

			var obj = new HashSet<int> { };
			var changed = new HashSet<int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void UnchangedNonEmpty()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}
	}
}
