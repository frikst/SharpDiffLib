﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.BaseWithId;
using SharpDiffLib.algorithms.applyPatch;
using SharpDiffLib.algorithms.diff;
using SharpDiffLib.algorithms.mergeDiffs;
using SharpDiffLib.algorithms.resolveConflicts;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;

namespace SharpDiffLib.Test.FullRun
{
	[TestClass]
	public class SubClass
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.BaseClassDiffRules()
					.Inheritable.ClassDiffRules()

					.MergeValueDiffsRules()
					.Inheritable.MergeClassDiffsRules()

					.ApplyValuePatchRules()
					.Inheritable.ApplyClassPatchRules()

					.ResolveAllSameRules(def => def
						.Action(ResolveAction.UseLeft)
					);
			}
		}

		[TestMethod]
		public void ConflictedProperty()
		{
			SampleBase @base = new SampleDescendant1();
			SampleBase left = new SampleDescendant1 { Value = "a" };
			SampleBase right = new SampleDescendant1 { Value = "b" };

			SampleBase ret = Merger.Instance.Merge(@base, left, right);
			Assert.IsInstanceOfType(ret, typeof(SampleDescendant1));
			Assert.AreEqual(((SampleDescendant1) ret).Value, "a");
		}

		[TestMethod]
		public void ChangedType()
		{
			SampleBase @base = new SampleDescendant1();
			SampleBase left = new SampleDescendant1();
			SampleBase right = new SampleDescendant2();

			SampleBase ret = Merger.Instance.Merge(@base, left, right);
			Assert.IsInstanceOfType(ret, typeof(SampleDescendant2));
		}

		[TestMethod]
		public void ChangedTypeWithConflict()
		{
			SampleBase @base = new SampleDescendant1();
			SampleBase left = new SampleDescendant1 { Value = "a" };
			SampleBase right = new SampleDescendant2();

			SampleBase ret = Merger.Instance.Merge(@base, left, right);
			Assert.IsInstanceOfType(ret, typeof(SampleDescendant1));
			Assert.AreEqual(((SampleDescendant1)ret).Value, "a");
		}
	}
}
