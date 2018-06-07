using System.Linq;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Factory;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ConflictResolver
{
	[TestFixture]
	public class ArrayOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{

			}
		}

		[Test]
		public void ConflictRemovedThenReplacedUseLeft()
		{
			var diffConflicted = DiffResultFactory.Ordered<int>()
				.Conflicted(
					c => c.Removed(0, 0),
					c => c.Replaced(0, 0, 5)
				)
				.Replaced(1, 3, 5)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>()
				.Removed(0, 0)
				.Replaced(1, 3, 5)
				.MakeDiff();

			var conflictResolver = Merger.Instance.Partial.GetConflictResolver(diffConflicted);

			Assert.IsTrue(conflictResolver.HasConflicts);

			IDiffItemConflicted[] conflicts = conflictResolver.ToArray();
			Assert.AreEqual(1, conflicts.Length);

			conflictResolver.ResolveConflict(conflicts[0], ResolveAction.UseLeft);

			Assert.IsFalse(conflictResolver.HasConflicts);
			Assert.AreEqual(diffResolved, conflictResolver.Finish());
		}

		[Test]
		public void ConflictRemovedThenReplacedUseRight()
		{
			var diffConflicted = DiffResultFactory.Ordered<int>()
				.Conflicted(
					c => c.Removed(0, 0),
					c => c.Replaced(0, 0, 5)
				)
				.Replaced(1, 3, 5)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>()
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

		[Test]
		public void ConflictRemovedRemovedThenReplacedUseLeft()
		{
			var diffConflicted = DiffResultFactory.Ordered<int>()
				.Conflicted(
					c => c.Removed(0, 0),
					c => c.Replaced(0, 0, 5)
				)
				.Removed(1, 1)
				.Replaced(2, 3, 5)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<int>()
				.Removed(0, 0)
				.Removed(1, 1)
				.Replaced(2, 3, 5)
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
