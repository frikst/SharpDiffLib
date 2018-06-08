using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.InnerStructure;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
	public class StructDiff
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleStruct>()
					.ClassDiffRules();

				Define<Sample>()
					.ClassDiffRules();

				Define<Dictionary<string, SampleStruct>>()
					.KeyValueCollectionDiffRules();
			}
		}

		[Test]
		public void StructChanged()
		{
			var @base = new SampleStruct { Value = 1 };
			var changed = new SampleStruct { Value = 2 };

			var expected = DiffFactory.Create<SampleStruct>().Class()
				.Replaced(x => x.Value, 1, 2)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void StructUnchanged()
		{
			var @base = new SampleStruct { Value = 1 };
			var changed = new SampleStruct { Value = 1 };

			var expected = DiffFactory.Create<SampleStruct>().Class()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void ClassChanged()
		{
			var @base = new Sample { Value = new SampleStruct { Value = 1 } };
			var changed = new Sample { Value = new SampleStruct { Value = 2 } };

			var expected = DiffFactory.Create<Sample>().Class()
				.Changed(x => x.Value, inner => inner.Class()
					.Replaced(x => x.Value, 1, 2)
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void ClassUnchanged()
		{
			var @base = new Sample { Value = new SampleStruct { Value = 1 } };
			var changed = new Sample { Value = new SampleStruct { Value = 1 } };

			var expected = DiffFactory.Create<Sample>().Class()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void DictionaryChanged()
		{
			var @base = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };
			var changed = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 2 } } };

			var expected = DiffFactory.Create<Dictionary<string, SampleStruct>>().KeyValue()
				.Changed("a", inner => inner.Class()
					.Replaced(x => x.Value, 1, 2)
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void DictionaryUnchanged()
		{
			var @base = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };
			var changed = new Dictionary<string, SampleStruct> { { "a", new SampleStruct { Value = 1 } } };

			var expected = DiffFactory.Create<Dictionary<string, SampleStruct>>().KeyValue()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
