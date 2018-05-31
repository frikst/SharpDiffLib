﻿using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.SharpDiffLib.Test.MergeDiffs
{
	[TestClass]
	public class SetOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<int>>()
					.MergeUnorderedCollectionDiffsRules();
			}
		}

		[TestMethod]
		public void EmptyDiffs()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyLeftAdded()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyRightAdded()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.Added(4)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.Added(4)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyBothAddedTheSame()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyRemovedReplaced()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Removed(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.Replaced(3, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Conflicted(
					c => c.Removed(3),
					c => c.Replaced(3, 5)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyBothRemovedTheSame()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Removed(3)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.Removed(3)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Removed(3)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyReplacedReplaced()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Replaced(3, 4)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.Replaced(3, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Conflicted(
					c => c.Replaced(3, 4),
					c => c.Replaced(3, 5)
				)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsTrue(conflicts.HasConflicts);
		}

		[TestMethod]
		public void EmptyReplacedReplacedBothSame()
		{
			var left = DiffResultFactory.Unordered<int>.Create()
				.Replaced(3, 5)
				.MakeDiff();
			var right = DiffResultFactory.Unordered<int>.Create()
				.Replaced(3, 5)
				.MakeDiff();

			IConflictContainer conflicts;
			var result = Merger.Instance.Partial.MergeDiffs(left, right, out conflicts);

			var merged = DiffResultFactory.Unordered<int>.Create()
				.Replaced(3, 5)
				.MakeDiff();

			Assert.AreEqual(merged, result);
			Assert.IsFalse(conflicts.HasConflicts);
		}
	}
}
