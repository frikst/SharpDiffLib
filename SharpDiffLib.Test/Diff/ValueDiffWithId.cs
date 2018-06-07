using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.InnerClassWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class ValueDiffWithId
	{
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

		[Test]
		public void TestClassReplaced()
		{
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

			var expected = DiffResultFactory.Class<Sample>()
				.Replaced(x => x.ValueInner, new SampleInner {Id = 1, Value = "one"}, new SampleInner {Id = 1, Value = "two"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestArrayReplaced()
		{
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

			var expected = DiffResultFactory.Ordered<SampleInner>()
				.Replaced(1, new SampleInner {Id = 2, Value = "b"}, new SampleInner {Id = 2, Value = "a"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestSetReplaced()
		{
			var @base = new HashSet<SampleInner> { new SampleInner { Id = 1, Value = "a" }, new SampleInner { Id = 2, Value = "b" } };
			var changed = new HashSet<SampleInner> { new SampleInner { Id = 1, Value = "a" }, new SampleInner { Id = 2, Value = "a" } };

			var expected = DiffResultFactory.Unordered<SampleInner>()
				.Replaced(new SampleInner {Id = 2, Value = "b"}, new SampleInner {Id = 2, Value = "a"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestDictionaryReplaced()
		{
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

			var expected = DiffResultFactory.KeyValue<string, SampleInner>()
				.Replaced("b", new SampleInner {Id = 2, Value = "b"}, new SampleInner {Id = 2, Value = "a"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
