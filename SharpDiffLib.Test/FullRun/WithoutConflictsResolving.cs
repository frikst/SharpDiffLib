using KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.DontResolve;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.FullRun
{
	[TestFixture]
	public class WithoutConflictsResolving
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>();
			}
		}

		[Test]
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

			Assert.AreEqual("a", ret.Value);
			Assert.IsNull(ret.Value2);
		}

		[Test]
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

			Assert.AreEqual("a", ret.Value);
			Assert.AreEqual("b", ret.Value2);
		}

		[Test]
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

			Assert.Throws<NotPossibleToResolveException>(() => Merger.Instance.Merge(@base, left, right));

		}
	}
}
