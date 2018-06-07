using System.Linq;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.InnerClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ConflictResolver
{
	[TestFixture]
	public class StructuredClass
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
			var diffConflicted = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Conflicted(
						c => c.Replaced(x => x.Value, "a", "b"),
						c => c.Replaced(x => x.Value, "a", "c")
					)
					.MakeDiff()
				)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Class<Sample>()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>()
					.Replaced(x => x.Value, "a", "b")
					.MakeDiff()
				)
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
