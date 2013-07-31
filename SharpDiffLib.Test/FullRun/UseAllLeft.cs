using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.SimpleClass;
using SharpDiffLib.algorithms.resolveConflicts;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;

namespace SharpDiffLib.Test.FullRun
{
	[TestClass]
	public class UseAllLeft
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.ResolveAllSameRules(def => def
						.Action(ResolveAction.UseLeft)
					);
			}
		}

		[TestMethod]
		public void SameChanges()
		{
			Sample @base = new Sample();

			Sample left = new Sample
			{
				Value = "a"
			};

			Sample right = new Sample
			{
				Value = "a"
			};

			Sample ret = Merger.Instance.Merge(@base, left, right);

			Assert.AreEqual(ret.Value, "a");
			Assert.IsNull(ret.Value2);
		}

		[TestMethod]
		public void Merged()
		{
			Sample @base = new Sample();

			Sample left = new Sample
			{
				Value = "a"
			};

			Sample right = new Sample
			{
				Value2 = "b"
			};

			Sample ret = Merger.Instance.Merge(@base, left, right);

			Assert.AreEqual(ret.Value, "a");
			Assert.AreEqual(ret.Value2, "b");
		}

		[TestMethod]
		public void Conflicted()
		{
			Sample @base = new Sample();

			Sample left = new Sample
			{
				Value = "a"
			};

			Sample right = new Sample
			{
				Value = "b"
			};

			Sample ret = Merger.Instance.Merge(@base, left, right);

			Assert.AreEqual(ret.Value, "a");
			Assert.IsNull(ret.Value2);
		}
	}
}
