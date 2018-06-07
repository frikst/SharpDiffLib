using System;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Test._Entities.InnerClassWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class StructuredClassDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleInner>()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					)
					.ClassDiffRules();

				Define<Sample>()
					.ClassDiffRules();
			}
		}

		[Test]
		public void TestInnerChanged()
		{
			string diff =
				"=ValueInner:" + Environment.NewLine +
				"\t-Value:one" + Environment.NewLine +
				"\t+Value:two";

			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
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

		[Test]
		public void TestInnerSame()
		{
			string diff = "";

			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestInnerReplaced()
		{
			string diff =
				"-ValueInner:<SampleInner:1>" + Environment.NewLine +
				"+ValueInner:<SampleInner:2>";

			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 2,
					Value = "one"
				}
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestInnerNullLeft()
		{
			string diff =
				"-ValueInner:(null)" + Environment.NewLine +
				"+ValueInner:<SampleInner:2>";

			var @base = new Sample
			{
				Value = "one",
				ValueInner = null
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 2,
					Value = "one"
				}
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestInnerNullRight()
		{
			string diff =
				"-ValueInner:<SampleInner:1>" + Environment.NewLine +
				"+ValueInner:(null)";

			var @base = new Sample
			{
				Value = "one",
				ValueInner = new SampleInner
				{
					Id = 1,
					Value = "one"
				}
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = null
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestInnerNullBoth()
		{
			string diff = "";

			var @base = new Sample
			{
				Value = "one",
				ValueInner = null
			};
			var changed = new Sample
			{
				Value = "one",
				ValueInner = null
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
