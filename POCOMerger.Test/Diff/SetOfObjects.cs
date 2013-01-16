using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.collection;
using POCOMerger.diff.common;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class SetOfObjects
	{
		private class Sample
		{
			public int Id { get; set; }

			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample:" + Id + ">";
			}
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<HashSet<Sample>>()
					.Rules<UnorderedCollectionDiffRules>();
				Define<Sample>()
					.Rules<GeneralRules<Sample>>(rules => rules
					    .Id(x => x.Id)
					)
					.Rules<ClassDiffRules>();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+<Sample:3>";
			var @base = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" } };
			var changed = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" }, new Sample { Id = 3, Value = "c" } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoAdded()
		{
			const string diff =
				"+<Sample:3>\r\n" +
				"+<Sample:4>";
			var @base = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" } };
			var changed = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" }, new Sample { Id = 3, Value = "c" }, new Sample { Id = 4, Value = "d" } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TwoRemoved()
		{
			const string diff =
				"-<Sample:3>\r\n" +
				"-<Sample:4>";
			var @base = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" }, new Sample { Id = 3, Value = "c" }, new Sample { Id = 4, Value = "d" } };
			var changed = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneChanged()
		{
			const string diff =
				"=1:\r\n" +
				"\t-Value:a\r\n" +
				"\t+Value:c";
			var @base = new HashSet<Sample> { new Sample { Id = 1, Value = "a" }, new Sample { Id = 2, Value = "b" } };
			var changed = new HashSet<Sample> { new Sample { Id = 1, Value = "c" }, new Sample { Id = 2, Value = "b" } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
