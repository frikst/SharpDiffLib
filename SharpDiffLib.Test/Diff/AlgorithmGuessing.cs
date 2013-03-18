using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDiffLib.Test._Entities.SimpleClass;
using SharpDiffLib.algorithms.diff.@base;
using SharpDiffLib.@base;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.Test.Diff
{
	[TestClass]
	public class AlgorithmGuessing
	{
		private class CustomDiffAlgorithmRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
		{
			#region Implementation of IDiffAlgorithmRules

			IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
			{
				return new CustomDiffAlgorithm<TType>();
			}

			#endregion
		}

		private class CustomDiffAlgorithm<TType> : IDiffAlgorithm<TType>
		{
			#region Implementation of IDiffAlgorithm

			public IDiff<TType> Compute(TType @base, TType changed)
			{
				return DiffResultFactory.Class<TType>.Create()
					// ignore differences
				    .MakeDiff();
			}

			bool IDiffAlgorithm.IsDirect
			{
				get { return false; }
			}
			IDiff IDiffAlgorithm.Compute(object @base, object changed)
			{
				return this.Compute((TType) @base, (TType) changed);
			}

			#endregion
		}

		private class Merger : MergerDefinition<Merger>
		{
			private Merger()
			{
				Define<Sample>();
			}

			protected override TAlgorithmRules RulesNotFoundFallback<TAlgorithmRules, TType>(IMergerRulesLocator rulesLocator)
			{
				if (typeof(TAlgorithmRules) == typeof(IDiffAlgorithmRules) && typeof(TType) == typeof(AnotherSample))
					return (TAlgorithmRules)(object)(new CustomDiffAlgorithmRules<TType>());
				return null;
			}
		}

		[TestMethod]
		public void TestInt()
		{
			const string diff =
				"-5\r\n" +
				"+0";

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestIntArray()
		{
			const string diff =
				"+2:3";
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestIntSet()
		{
			const string diff =
				"+3";
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestDifferentObjects()
		{
			const string diff =
				"-Value:one\r\n" +
				"+Value:two";

			var @base = new Sample { Value = "one" };
			var changed = new Sample { Value = "two" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[TestMethod]
		public void TestCustomGuess()
		{
			const string diff = "";

			var @base = new AnotherSample { AnotherValue = "one" };
			var changed = new AnotherSample { AnotherValue = "two" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
