﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.algorithms.applyPatch;
using SharpDiffLib.definition;
using SharpDiffLib.diffResult;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.Test.ApplyPatch
{
	[TestClass]
	public class DictionaryOfPrimitives
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Dictionary<string, int>>()
					.ApplyKeyValueCollectionPatchRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.Added("c", 3)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.Added("c", 3)
				.Added("d", 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void TwoRemoved()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.Removed("c", 3)
				.Removed("d", 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneReplaced()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.Replaced("c", 3, 4)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 4 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllAdded()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.Added("a", 1)
				.Added("b", 2)
				.MakeDiff();

			var obj = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void AllRemoved()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.Removed("a", 1)
				.Removed("b", 2)
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void UnchangedEmpty()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.MakeDiff();

			var obj = new Dictionary<string, int> { };
			var changed = new Dictionary<string, int> { };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void UnchangedNonEmpty()
		{
			var diff = DiffResultFactory.KeyValue<string, int>.Create()
				.MakeDiff();

			var obj = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
			var changed = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}