using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOMerger.definition;
using POCOMerger.diff.common;

namespace POCOMerger.Test.Diff
{
	[TestClass]
	public class SimpleClassDiff
	{
		private class Sample
		{
			public string Value { get; set; }
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>()
					.Diff(x => 
						x.Using<ClassDiff>()
					);
			}
		}

		[TestMethod]
		public void TestDifferent()
		{
			var ret = Merger.Instance.Partial.Diff(new Sample { Value = "one" }, new Sample { Value = "two" }).ToList();
		}
	}
}
