using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Test._Entities.InnerStructure;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class StructDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleStruct>()
					.ClassDiffRules();

				Define<Sample>()
					.ClassDiffRules();

				Define<Dictionary<string, SampleStruct>>()
					.KeyValueCollectionDiffRules();
			}
		}

		[Test]
		public void StructChanged()
		{
			string diff =
				"-Value:1" + Environment.NewLine +
				"+Value:2";
			var @base = new SampleStruct { Value = 1 };
			var changed = new SampleStruct { Value = 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void StructUnchanged()
		{
			string diff = "";
			var @base = new SampleStruct { Value = 1 };
			var changed = new SampleStruct { Value = 1 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void ClassChanged()
		{
			string diff =
				"=Value:" + Environment.NewLine +
				"\t-Value:1" + Environment.NewLine +
				"\t+Value:2";
			var @base = new Sample { Value = new SampleStruct { Value = 1 } };
			var changed = new Sample { Value = new SampleStruct { Value = 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void ClassUnchanged()
		{
			string diff = "";
			var @base = new Sample { Value = new SampleStruct { Value = 1 } };
			var changed = new Sample { Value = new SampleStruct { Value = 1 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void DictionaryChanged()
		{
			string diff =
				"=a:" + Environment.NewLine +
				"\t-Value:1" + Environment.NewLine +
				"\t+Value:2";
			var @base = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };
			var changed = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 2 } } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void DictionaryUnchanged()
		{
			string diff = "";
			var @base = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };
			var changed = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
