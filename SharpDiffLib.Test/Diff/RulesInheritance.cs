using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Test._Entities.BaseWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class RulesInheritance
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.Inheritable.ClassDiffRules()
					.Inheritable.GeneralRules(rules => rules
						.Id(x => x.Id)
					);

				Define<SampleDescendant1[]>()
					.OrderedCollectionDiffRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			const string diff =
				"+1:<Sample1:2>";
			var @base = new[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 3, Value = "c" }
			};
			var changed = new[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" },
				new SampleDescendant1 { Id = 3, Value = "c" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void OneChanged()
		{
			const string diff =
				"=0:\r\n" +
				"\t-Value:a\r\n" +
				"\t+Value:b";
			var @base = new[]
			{
				new SampleDescendant1 { Id = 0, Value = "a" },
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};
			var changed = new[]
			{
				new SampleDescendant1 { Id = 0, Value = "b" },
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
