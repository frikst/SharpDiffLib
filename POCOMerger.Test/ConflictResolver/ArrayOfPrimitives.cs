using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.conflictManagement;
using POCOMerger.definition;
using POCOMerger.diffResult;
using POCOMerger.diffResult.action;

namespace POCOMerger.Test.ConflictResolver
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
				.Added(0, 3)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>.Create()
				.Removed(0, 0)
				.Added(0, 3)
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
				.Added(0, 3)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>.Create()
				.Replaced(0, 0, 5)
				.Added(1, 3)
				.MakeDiff();

			var conflictResolver = Merger.Instance.Partial.GetConflictResolver(diffConflicted);

			Assert.IsTrue(conflictResolver.HasConflicts);

			IDiffItemConflicted[] conflicts = conflictResolver.ToArray();
			Assert.AreEqual(1, conflicts.Length);

			conflictResolver.ResolveConflict(conflicts[0], ResolveAction.UseRight);

			Assert.IsFalse(conflictResolver.HasConflicts);
			Assert.AreEqual(diffResolved, conflictResolver.Finish());
		}
	}
}
