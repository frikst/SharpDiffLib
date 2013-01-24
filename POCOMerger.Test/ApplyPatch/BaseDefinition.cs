using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.algorithms.applyPatch;
using POCOMerger.algorithms.diff;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diffResult;
using POCOMerger.diffResult.@base;

namespace POCOMerger.Test.ApplyPatch
{
	[TestClass]
	public class BaseDefinition
	{
		private abstract class SampleBase
		{
			public int Id { get; set; }

			#region Equality members

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj))
					return false;
				if (ReferenceEquals(this, obj))
					return true;
				if (!(obj is SampleBase))
					return false;

				return this.Id == ((SampleBase) obj).Id
					&& this.GetType() == obj.GetType();
			}

			public override int GetHashCode()
			{
				return this.Id;
			}

			#endregion
		}

		private class Sample1 : SampleBase
		{
			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample1:" + Id + ">";
			}
		}

		private class Sample2 : SampleBase
		{
			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample2:" + Id + ">";
			}
		}

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
					.ApplyOrderedCollectionPatchRules();
			}
		}

		[TestMethod]
		public void OneAdded()
		{
			var diff = DiffResultFactory.Ordered<SampleBase>.Create()
				.Added(1, new Sample1 { Id = 2, Value = "b" })
				.MakeDiff();

			var obj = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" }
			};
			var changed = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneReplacedWithOtherDescendant()
		{
			var diff = DiffResultFactory.Ordered<SampleBase>.Create()
				.Changed(1, DiffResultFactory.Value<SampleBase>.Create()
					.Replaced(
						new Sample1 { Id = 2, Value = "b" },
						new Sample2 { Id = 2, Value = "b" }
					)
					.MakeDiff()
				)
				.MakeDiff();

			var obj = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample2 { Id = 2, Value = "b" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}

		[TestMethod]
		public void OneChangedProperty()
		{
			var diff = DiffResultFactory.Ordered<SampleBase>.Create()
				.Changed(1, DiffResultFactory.Value<SampleBase>.Create()
					.Changed(DiffResultFactory.Class<Sample1>.Create()
						.Replaced(x => x.Value, "b", "c")
						.MakeDiff()
					)
					.MakeDiff()
				)
				.MakeDiff();

			var obj = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "b" }
			};
			var changed = new SampleBase[]
			{
				new Sample1 { Id = 1, Value = "a" },
				new Sample1 { Id = 2, Value = "c" }
			};

			var ret = Merger.Instance.Partial.ApplyPatch(obj, diff);

			CollectionAssert.AreEqual(changed, ret);
		}
	}
}
