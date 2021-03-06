﻿using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
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

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Added("c", new Sample {Id = 3, Value = "c"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoAdded()
		{
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

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Added("c", new Sample {Id = 3, Value = "c"})
				.Added("d", new Sample {Id = 4, Value = "d"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TwoRemoved()
		{
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

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Removed("c", new Sample {Id = 3, Value = "c"})
				.Removed("d", new Sample {Id = 4, Value = "d"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneReplaced()
		{
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

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Replaced("c", new Sample {Id = 3, Value = "c"}, new Sample {Id = 4, Value = "d"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void OneChanged()
		{
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

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Changed("c", inner => inner.Class()
					.Replaced(x => x.Value, "c", "d")
				)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllAdded()
		{
			var @base = new Dictionary<string, Sample> { };
			var changed = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Added("a", new Sample {Id = 1, Value = "a"})
				.Added("b", new Sample {Id = 2, Value = "b"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void AllRemoved()
		{
			var @base = new Dictionary<string, Sample>
			{
				{ "a", new Sample { Id = 1, Value = "a" } },
				{ "b", new Sample { Id = 2, Value = "b" } }
			};
			var changed = new Dictionary<string, Sample> { };

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.Removed("a", new Sample {Id = 1, Value = "a"})
				.Removed("b", new Sample {Id = 2, Value = "b"})
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void UnchangedEmpty()
		{
			var @base = new Dictionary<string, Sample> { };
			var changed = new Dictionary<string, Sample> { };

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void UnchangedNonEmpty()
		{
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

			var expected = DiffFactory.Create<Dictionary<string, Sample>>().KeyValue()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
