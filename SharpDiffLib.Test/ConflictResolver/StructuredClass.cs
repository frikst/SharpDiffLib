﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.InnerClass;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;
using SharpDiffLib.diffResult.action;

namespace SharpDiffLib.Test.ConflictResolver
{
	[TestClass]
	public class StructuredClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				
			}
		}

		[TestMethod]
		public void OneConflictUseLeft()
		{
			var diffConflicted = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
					.Conflicted(
						c => c.Replaced(x => x.Value, "a", "b"),
						c => c.Replaced(x => x.Value, "a", "c")
					)
					.MakeDiff()
				)
				.MakeDiff();

			var diffResolved = DiffResultFactory.Class<Sample>.Create()
				.Changed(x => x.ValueInner, DiffResultFactory.Class<SampleInner>.Create()
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