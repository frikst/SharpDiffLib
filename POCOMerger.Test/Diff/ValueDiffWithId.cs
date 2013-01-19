using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class ValueDiffWithId
	{
		private class SampleInner
		{
			public int Id { get; set; }
			public string Value { get; set; }

			public override string ToString()
			{
				return "<SampleInner:" + this.Id + ">";
			}

			public override bool Equals(object obj)
			{
				if (!(obj is SampleInner))
					return false;
				SampleInner s = (SampleInner) obj;

				if (s.Id != this.Id)
					return false;

				if (s.Value != this.Value)
					return false;

				return true;
			}
		}

		private class Sample
		{
			public SampleInner ValueInner { get; set; }

			public override string ToString()
			{
				return "<Sample>";
			}
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleInner>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.ValueDiffRules();

				Define<Sample>()
					.ClassDiffRules();

				Define<SampleInner[]>()
					.OrderedCollectionDiffRules();

				Define<HashSet<SampleInner>>()
					.UnorderedCollectionDiffRules();

				Define<Dictionary<string, SampleInner>>()
					.KeyValueCollectionDiffRules();
			}
		}

		[TestMethod]
		public void TestClassReplaced()
		{
			const string diff =
				"-ValueInner:<SampleInner:1>\r\n" +
				"+ValueInner:<SampleInner:1>";

			var @base = new Sample
			{
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "two"
				}
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestArrayReplaced()
		{
			const string diff =
				"-1:<SampleInner:2>\r\n" +
				"+1:<SampleInner:2>";
			var @base = new[]
			{
				new SampleInner { Id = 1, Value = "a" },
				new SampleInner { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new SampleInner { Id = 1, Value = "a" },
				new SampleInner { Id = 2, Value = "a" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestSetReplaced()
		{
			const string diff =
				"-<SampleInner:2>\r\n" +
				"+<SampleInner:2>";
			var @base = new HashSet<SampleInner> { new SampleInner { Id = 1, Value = "a" }, new SampleInner { Id = 2, Value = "b" } };
			var changed = new HashSet<SampleInner> { new SampleInner { Id = 1, Value = "a" }, new SampleInner { Id = 2, Value = "a" } };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestDictionaryReplaced()
		{
			const string diff =
				"-b:<SampleInner:2>\r\n" +
				"+b:<SampleInner:2>";

			var @base = new Dictionary<string, SampleInner>
			{
				{ "a", new SampleInner { Id = 1, Value = "a" } },
				{ "b", new SampleInner { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, SampleInner>
			{
				{ "a", new SampleInner { Id = 1, Value = "a" } },
				{ "b", new SampleInner { Id = 2, Value = "a" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
