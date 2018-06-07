using System;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
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

		[Test]
		public void TestIntDifferent()
		{
			string diff =
				"-<Sample:5>" + Environment.NewLine +
				"+<Sample:0>";

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 0, Value = "b" });

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestSameValues()
		{
			string diff = "";

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 5, Value = "a" });

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
