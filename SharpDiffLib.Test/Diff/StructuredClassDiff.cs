﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.InnerClassWithId;
using SharpDiffLib.algorithms.diff;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;

namespace SharpDiffLib.Test.Diff
{
	[TestClass]
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

		[TestMethod]
		public void TestInnerChanged()
		{
			const string diff =
				"=ValueInner:\r\n" +
				"\t-Value:one\r\n" +
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

		[TestMethod]
		public void TestInnerSame()
		{
			const string diff = "";

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

		[TestMethod]
		public void TestInnerReplaced()
		{
			const string diff =
				"-ValueInner:<SampleInner:1>\r\n" +
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

		[TestMethod]
		public void TestInnerNullLeft()
		{
			const string diff =
				"-ValueInner:(null)\r\n" +
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

		[TestMethod]
		public void TestInnerNullRight()
		{
			const string diff =
				"-ValueInner:<SampleInner:1>\r\n" +
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

		[TestMethod]
		public void TestInnerNullBoth()
		{
			const string diff = "";

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