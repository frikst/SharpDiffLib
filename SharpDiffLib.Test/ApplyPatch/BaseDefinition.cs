using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Factory;
using KST.SharpDiffLib.Test._Entities.BaseWithId;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.ApplyPatch
{
	[TestFixture]
	public class BaseDefinition
	{
		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<SampleBase>()
					.ApplyValuePatchRules()
					.Inheritable.ApplyClassPatchRules()
					.Inheritable.GeneralRules(rules => rules
						.Id(x => x.Id)
					);

				Define<SampleBase[]>()
					.Inheritable.ApplyOrderedCollectionPatchRules();
			}
		}

		[Test]
		public void OneAdded()
		{
			var diff = DiffFactory.Create<SampleBase[]>().Ordered()
				.Added(1, new SampleDescendant1 { Id = 2, Value = "b" })
				.MakeDiff();

			var obj = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" }
			};
			var changed = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneReplacedWithOtherDescendant()
		{
			var diff = DiffFactory.Create<SampleBase[]>().Ordered()
				.Changed(1, inner => inner.Value()
					.Replaced(
						new SampleDescendant1 { Id = 2, Value = "b" },
						new SampleDescendant2 { Id = 2, Value = "b" }
					)
				)
				.MakeDiff();

			var obj = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant2 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[Test]
		public void OneChangedProperty()
		{
			var diff = DiffFactory.Create<SampleBase[]>().Ordered()
				.Changed(1, inner => inner.Value()
					.ChangedType<SampleDescendant1>(inner2 => inner2.Class()
						.Replaced(x => x.Value, "b", "c")
					)
				)
				.MakeDiff();

			var obj = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new SampleDescendant1 { Id = 1, Value = "a" },
				new SampleDescendant1 { Id = 2, Value = "c" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
