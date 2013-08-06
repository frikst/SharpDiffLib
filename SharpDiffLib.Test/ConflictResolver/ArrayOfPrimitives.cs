﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;
using SharpDiffLib.diffResult.action;

namespace SharpDiffLib.Test.ConflictResolver
{
	[TestClass]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{

			}
		}

		[TestMethod]
		public void ConflictRemovedThenReplacedUseLeft()
		{
			var diffConflicted = DiffResultFactory.Ordered<int>.Create()
				.Conflicted(
					c => c.Removed(0, 0),
					c => c.Replaced(0, 0, 5)
				)
				.Replaced(0, 3, 5)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>.Create()
				.Removed(0, 0)
				.Replaced(0, 3, 5)
				.MakeDiff();

			var conflictResolver = Merger.Instance.Partial.GetConflictResolver(diffConflicted);

			Assert.IsTrue(conflictResolver.HasConflicts);

			IDiffItemConflicted[] conflicts = conflictResolver.ToArray();
			Assert.AreEqual(1, conflicts.Length);

			conflictResolver.ResolveConflict(conflicts[0], ResolveAction.UseLeft);

			Assert.IsFalse(conflictResolver.HasConflicts);
			Assert.AreEqual(diffResolved, conflictResolver.Finish());
		}

		[TestMethod]
		public void ConflictRemovedThenReplacedUseRight()
		{
			var diffConflicted = DiffResultFactory.Ordered<int>.Create()
				.Conflicted(
					c => c.Removed(0, 0),
					c => c.Replaced(0, 0, 5)
				)
				.Replaced(0, 3, 5)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>.Create()
				.Replaced(0, 0, 5)
				.Replaced(1, 3, 5)
				.MakeDiff();

			var conflictResolver = Merger.Instance.Partial.GetConflictResolver(diffConflicted);

			Assert.IsTrue(conflictResolver.HasConflicts);

			IDiffItemConflicted[] conflicts = conflictResolver.ToArray();
			Assert.AreEqual(1, conflicts.Length);

			conflictResolver.ResolveConflict(conflicts[0], ResolveAction.UseRight);

			Assert.IsFalse(conflictResolver.HasConflicts);
			Assert.AreEqual(diffResolved, conflictResolver.Finish());
		}

		[TestMethod]
		public void ConflictRemovedRemovedThenReplacedUseLeft()
		{
			var diffConflicted = DiffResultFactory.Ordered<int>.Create()
				.Conflicted(
					c => c
						.Removed(0, 0)
						.Removed(0, 1),
					c => c.Replaced(0, 0, 5)
				)
				.Replaced(0, 3, 5)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>.Create()
				.Removed(0, 0)
				.Removed(0, 1)
				.Replaced(0, 3, 5)
				.MakeDiff();

			var conflictResolver = Merger.Instance.Partial.GetConflictResolver(diffConflicted);

			Assert.IsTrue(conflictResolver.HasConflicts);

			IDiffItemConflicted[] conflicts = conflictResolver.ToArray();
			Assert.AreEqual(1, conflicts.Length);

			conflictResolver.ResolveConflict(conflicts[0], ResolveAction.UseLeft);

			Assert.IsFalse(conflictResolver.HasConflicts);
			Assert.AreEqual(diffResolved, conflictResolver.Finish());
		}
	}
}