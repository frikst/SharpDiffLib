﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;
using SharpDiffLib.Test._Entities.SimpleClass;

namespace SharpDiffLib.Test.ApplyPatch
{
	[TestClass]
	public class AlgorithmGuessing
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>();
			}
		}

		[TestMethod]
		public void TestInt()
		{
			var diff = DiffResultFactory.Value<int>.Create()
			    .Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.ApplyPatch(5, diff);

			Assert.AreEqual(0, ret);
		}

		[TestMethod]
		public void TestIntArray()
		{
			var diff = DiffResultFactory.Ordered<int>.Create()
				.Added(2, 3)
				.MakeDiff();

			var obj = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TestIntSet()
		{
			var diff = DiffResultFactory.Unordered<int>.Create()
				.Added(3)
				.MakeDiff();

			var obj = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed.OrderBy(x => x).ToArray(), ret.OrderBy(x => x).ToArray());
		}

		[TestMethod]
		public void TestDifferentObjects()
		{
			var diff = DiffResultFactory.Class<Sample>.Create()
				.Replaced(x => x.Value, "one", "two")
				.MakeDiff();

			var obj = new Sample { Value = "one" };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			Assert.AreEqual("two", ret.Value);
		}
	}
}
