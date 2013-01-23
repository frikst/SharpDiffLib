using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.diff;
using POCOMerger.definition;
using POCOMerger.definition.rules;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class BaseDefinition
	{
		private abstract class SampleBase
		{
			public int Id { get; set; }
		}

		private class Sample1 : SampleBase
		{
			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample1:" + Id + ">";
			}
		}

		private class Sample2 : SampleBase
		{
			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample2:" + Id + ">";
			}
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.BaseClassDiffRules()
					.Inheritable.ClassDiffRules()
					.Inheritable.GeneralRules(rules => rules
						.Id(x => x.Id)
					);

				Define<SampleBase[]>()
					.OrderedCollectionDiffRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			const string diff =
				"+1:<Sample1:2>";
			var @base = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" }
			};
			var changed = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneReplacedWithOtherDescendant()
		{
			const string diff =
				"=1:\r\n" +
				"\t-<Sample1:2>\r\n" +
				"\t+<Sample2:2>";
			var @base = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample2 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void OneChangedProperty()
		{
			const string diff =
				"=1:\r\n" +
				"\t-Value:b\r\n" +
				"\t+Value:c";
			var @base = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "c" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
