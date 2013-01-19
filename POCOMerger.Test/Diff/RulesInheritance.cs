using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.collection;
using POCOMerger.diff.collection.ordered;
using POCOMerger.diff.common;
using POCOMerger.diff.common.@class;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class RulesInheritance
	{
		private class SampleBase
		{
			public int Id { get; set; }

			public override string ToString()
			{
				return "<Sample:" + Id + ">";
			}
		}

		private class Sample : SampleBase
		{
			public string Value { get; set; }
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.Inheritable.Rules<ClassDiffRules>()
					.Inheritable.Rules<GeneralRules<SampleBase>>(rules => rules
						.Id(x => x.Id)
					);

				Define<Sample[]>()
					.Rules<OrderedCollectionDiffRules>();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+1:<Sample:2>";
			var @base = new[]
			{
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 3, Value = "c" }
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
		public void OneChanged()
		{
			const string diff =
				"=0:\r\n" +
				"\t-Value:a\r\n" +
				"\t+Value:b";
			var @base = new[]
			{
				new Sample { Id = 0, Value = "a" },
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new Sample { Id = 0, Value = "b" },
				new Sample { Id = 1, Value = "a" },
				new Sample { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
