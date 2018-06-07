using System.Collections.Generic;
using System.Linq;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class AlgorithmGuessing
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>();
			}
		}

		[Test]
		public void TestInt()
		{
			var diff = DiffResultFactory.Value<int>()
			    .Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.ApplyPatch(5, diff);

			Assert.AreEqual(0, ret);
		}

		[Test]
		public void TestIntArray()
		{
			var diff = DiffResultFactory.Ordered<int>()
				.Added(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void TestIntSet()
		{
			var diff = DiffResultFactory.Unordered<int>()
				.Added(3)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[Test]
		public void TestDifferentObjects()
		{
			var diff = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.Value, "one", "two")
				.MakeDiff();

			var obj = new Sample { Value = "one" };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("two", ret.Value);
		}
	}
}
