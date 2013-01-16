using System;
using System.Linq.Expressions;
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
	public class ArrayOfObjects
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
				Define<Sample[]>()
					.Rules<OrderedCollectionDiffRules>();

				Define<Sample>()
					.Rules<ClassDiffRules>()
					.Rules<GeneralRules<Sample>>(rules => rules
						.Id(x => x.Id)
					);
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+2:<Sample:3>";
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" },
				new Sample { Id = 3, Value = "c" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneRemoved()
		{
			const string diff =
				"-0:<Sample:0>";
			var @base = new[]
			{
				new Sample { Id = 0, Value = "a" },
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneChanged()
		{
			const string diff =
				"=0:\r\n" +
				"\t-Value:a\r\n" +
				"\t+Value:c";
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "c" },
				new Sample { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneReplacedWithNull()
		{
			const string diff =
				"-1:<Sample:2>\r\n" +
				"+1:(null)";
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 1, Value = "a" },
				null
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
