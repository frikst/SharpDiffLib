using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.Test._Entities.SimpleWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class DictionaryOfObjects
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Dictionary<string, Sample>>()
					.KeyValueCollectionDiffRules();

				Define<Sample>()
					.ClassDiffRules()
					.GeneralRules(rules => rules
						.Id(x => x.Id)
					);
			}
		}

		[Test]
		public void OneAdded()
		{
			const string diff =
				"+c:<Sample:3>";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoAdded()
		{
			const string diff =
				"+c:<Sample:3>\r\n" +
				"+d:<Sample:4>";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } },
				{ "d", new Sample { Id = 4, Value = "d" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TwoRemoved()
		{
			const string diff =
				"-c:<Sample:3>\r\n" +
				"-d:<Sample:4>";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } },
				{ "d", new Sample { Id = 4, Value = "d" } },
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void OneReplaced()
		{
			const string diff =
				"-c:<Sample:3>\r\n" +
				"+c:<Sample:4>";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 4, Value = "d" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void OneChanged()
		{
			const string diff =
				"=c:\r\n" +
				"\t-Value:c\r\n" +
				"\t+Value:d";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "c" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } },
				{ "c", new Sample { Id = 3, Value = "d" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllAdded()
		{
			const string diff =
				"+a:<Sample:1>\r\n" +
				"+b:<Sample:2>";

			var @base = new Dictionary<string, Sample> { };
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void AllRemoved()
		{
			const string diff =
				"-a:<Sample:1>\r\n" +
				"-b:<Sample:2>";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, Sample> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(2, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void UnchangedEmpty()
		{
			const string diff = "";

			var @base = new Dictionary<string, Sample> { };
			var changed = new Dictionary<string, Sample> { };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void UnchangedNonEmpty()
		{
			const string diff = "";

			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
