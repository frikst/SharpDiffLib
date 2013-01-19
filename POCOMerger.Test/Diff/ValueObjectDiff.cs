﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.common;
using POCOMerger.diff.common.value;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class ValueObjectDiff
	{
		private class Sample
		{
			public int Id { get; set; }
			public string Value { get; set; }

			public override string ToString()
			{
				return "<Sample:" + this.Id + ">";
			}
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.Rules<GeneralRules<Sample>>(rules => rules
						.Id(x => x.Id)
					)
					.Rules<ValueDiffRules>();
				Define<int>()
					.Rules<ValueDiffRules>();
			}
		}

		[TestMethod]
		public void TestIntDifferent()
		{
			const string diff =
				"-<Sample:5>\r\n" +
				"+<Sample:0>";

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 0, Value = "b" });

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestSameValues()
		{
			const string diff = "";

			var ret = Merger.Instance.Partial.Diff(new Sample { Id = 5, Value = "a" }, new Sample { Id = 5, Value = "a" });

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
