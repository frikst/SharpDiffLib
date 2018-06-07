using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult;
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
			}
		}

		[Test]
		public void TestIntDifferent()
		{
			var expected = DiffResultFactory.Value<Sample>.Create()
				.Replaced(new Sample {Id = 5, Value = "a"}, new Sample {Id = 0, Value = "b"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 0, Value = "b" });

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestSameValues()
		{
			var expected = DiffResultFactory.Value<Sample>.Create()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 5, Value = "a" });

			Assert.AreEqual(expected, ret);
		}
	}
}
