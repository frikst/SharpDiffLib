using System.Linq;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ConflictResolver
{
	[TestFixture]
	public class SimpleClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				
			}
		}

		[Test]
		public void OneConflictUseLeft()
		{
			var diffConflicted = DiffResultFactory.Class<Sample>.Create()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "b")
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
		public void OneConflictUseRight()
		{
			var diffConflicted = DiffResultFactory.Class<Sample>.Create()
				.Conflicted(
					c => c.Replaced(x => x.Value, "a", "b"),
					c => c.Replaced(x => x.Value, "a", "c")
				)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "a", "c")
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
