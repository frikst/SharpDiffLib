using KST.SharpDiffLib.Algorithms.ApplyPatch;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult;
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
			var diff = DiffResultFactory.Ordered<SampleBase>.Create()
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
			var diff = DiffResultFactory.Ordered<SampleBase>.Create()
				.Changed(1, DiffResultFactory.Value<SampleBase>.Create()
					.Replaced(
						new SampleDescendant1 { Id = 2, Value = "b" },
						new SampleDescendant2 { Id = 2, Value = "b" }
					)
					.MakeDiff()
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
			var diff = DiffResultFactory.Ordered<SampleBase>.Create()
				.Changed(1, DiffResultFactory.Value<SampleBase>.Create()
					.Changed(DiffResultFactory.Class<SampleDescendant1>.Create()
						.Replaced(x => x.Value, "b", "c")
						.MakeDiff()
					)
					.MakeDiff()
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
