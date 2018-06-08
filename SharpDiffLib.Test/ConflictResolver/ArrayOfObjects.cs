using System.Linq;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ConflictResolver
{
	[TestFixture]
	public class ArrayOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{

			}
		}

		[Test]
		public void TestReplacedConflictedUseLeft()
		{
			var diffConflicted = DiffFactory.Create<Sample[]>().Ordered()
				.Conflicted(
					c => c.Removed(0, new Sample { Id = 1 }),
					c => c.Changed(0, inner => inner.Class()
						.Replaced(x => x.Value, "a", "b")
					)
				)
				.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			var diffResolved = DiffFactory.Create<Sample[]>().Ordered()
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

		[Test]
		public void TestReplacedConflictedUseRight()
		{
			var diffConflicted = DiffFactory.Create<Sample[]>().Ordered()
				.Conflicted(
					c => c.Removed(0, new Sample { Id = 1 }),
					c => c.Changed(0, inner => inner.Class()
						.Replaced(x => x.Value, "a", "b")
					)
				)
				.Added(1, new Sample { Id = 2 })
				.MakeDiff();

			var diffResolved = DiffFactory.Create<Sample[]>().Ordered()
				.Changed(0, inner => inner.Class()
					.Replaced(x => x.Value, "a", "b")
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
