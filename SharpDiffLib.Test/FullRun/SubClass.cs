using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Algorithms.MergeDiffs;
using KST.SharpDiffLib.Algorithms.ResolveConflicts;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Test._Entities.BaseWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.FullRun
{
	[TestFixture]
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

		[Test]
		public void ConflictedProperty()
		{
			SampleBase @base = new SampleDescendant1();
			SampleBase left = new SampleDescendant1 { Value = "a" };
			SampleBase right = new SampleDescendant1 { Value = "b" };

			SampleBase ret = Merger.Instance.Merge(@base, left, right);
			Assert.IsInstanceOf<SampleDescendant1>(ret);
			Assert.AreEqual("a", ((SampleDescendant1)ret).Value);
		}

		[Test]
		public void ChangedType()
		{
			SampleBase @base = new SampleDescendant1();
			SampleBase left = new SampleDescendant1();
			SampleBase right = new SampleDescendant2();

			SampleBase ret = Merger.Instance.Merge(@base, left, right);
			Assert.IsInstanceOf<SampleDescendant2>(ret);
		}

		[Test]
		public void ChangedTypeWithConflict()
		{
			SampleBase @base = new SampleDescendant1();
			SampleBase left = new SampleDescendant1 { Value = "a" };
			SampleBase right = new SampleDescendant2();

			SampleBase ret = Merger.Instance.Merge(@base, left, right);
			Assert.IsInstanceOf<SampleDescendant1>(ret);
			Assert.AreEqual("a", ((SampleDescendant1)ret).Value);
		}
	}
}
