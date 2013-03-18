using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.algorithms.applyPatch;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.Test.ApplyPatch
{
	[TestClass]
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

		[TestMethod]
		public void OneAdded()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
			    .MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void TwoAdded()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.Added(4)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3, 4 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void TwoRemoved()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.Removed(3)
				.Removed(4)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2, 3, 4 };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void AllAdded()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.Added(1)
				.Added(2)
				.MakeDiff();

			var obj = new HashSet<int> { };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void AllRemoved()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.Removed(1)
				.Removed(2)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void UnchangedEmpty()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			var obj = new HashSet<int> { };
			var changed = new HashSet<int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void UnchangedNonEmpty()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}
	}
}
