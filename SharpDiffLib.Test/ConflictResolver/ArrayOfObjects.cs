using System.Linq;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KST.SharpDiffLib.Test.ConflictResolver
{
	[TestClass]
	public class ArrayOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{

			}
		}

		[TestMethod]
		public void TestReplacedConflictedUseLeft()
		{
			var diffConflicted = DiffResultFactory.Ordered<Sample>.Create()
				.Conflicted(
					c => c.Removed(0, new Sample { Id = 1 }),
					c => c.Changed(0, DiffResultFactory.Class<Sample>.Create()
						.Replaced(x => x.Value, "a", "b")
						.MakeDiff()
					)
				)
				.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<Sample>.Create()
				.Removed(0, new Sample { Id = 1 })
				.Added(1, new Sample { Id = 2 })
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
		public void TestReplacedConflictedUseRight()
		{
			var diffConflicted = DiffResultFactory.Ordered<Sample>.Create()
				.Conflicted(
					c => c.Removed(0, new Sample { Id = 1 }),
					c => c.Changed(0, DiffResultFactory.Class<Sample>.Create()
						.Replaced(x => x.Value, "a", "b")
						.MakeDiff()
					)
				)
				.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			var diffResolved = DiffResultFactory.Ordered<Sample>.Create()
				.Changed(0, DiffResultFactory.Class<Sample>.Create()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
				.Added(1, new Sample { Id = 2 })
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
