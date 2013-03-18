using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.SimpleWithId;
using SharpDiffLib.algorithms.diff;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.Test.Diff
{
	[TestClass]
	public class ValueObjectDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.ValueDiffRules();
				Define<int>()
					.ValueDiffRules();
			}
		}

		[TestMethod]
		public void TestIntDifferent()
		{
			const string diff =
				"-<Sample:5>\r\n" +
				"+<Sample:0>";

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 0, Value = "b" });

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestSameValues()
		{
			const string diff = "";

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 5, Value = "a" });

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
