﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.diff;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class StructDiff
	{
		public struct SampleStruct
		{
			public int Value { get; set; }
		}

		private class Sample
		{
			public SampleStruct Value { get; set; }

			public override string ToString()
			{
				return "<Sample>";
			}
		}

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

		[TestMethod]
		public void StructChanged()
		{
			const string diff =
				"-Value:1\r\n" +
				"+Value:2";
			var @base = new SampleStruct { Value = 1 };
			var changed = new SampleStruct { Value = 2 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void StructUnchanged()
		{
			const string diff = "";
			var @base = new SampleStruct { Value = 1 };
			var changed = new SampleStruct { Value = 1 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void ClassChanged()
		{
			const string diff =
				"=Value:\r\n" +
				"\t-Value:1\r\n" +
				"\t+Value:2";
			var @base = new Sample { Value = new SampleStruct { Value = 1 } };
			var changed = new Sample { Value = new SampleStruct { Value = 2 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void ClassUnchanged()
		{
			const string diff = "";
			var @base = new Sample { Value = new SampleStruct { Value = 1 } };
			var changed = new Sample { Value = new SampleStruct { Value = 1 } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void DictionaryChanged()
		{
			const string diff =
				"=a:\r\n" +
				"\t-Value:1\r\n" +
				"\t+Value:2";
			var @base = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };
			var changed = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 2 } } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void DictionaryUnchanged()
		{
			const string diff = "";
			var @base = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };
			var changed = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
