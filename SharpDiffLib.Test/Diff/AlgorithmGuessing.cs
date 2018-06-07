using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.Test._Entities.SimpleClass;
using NUnit.Framework;

namespace KST.SharpDiffLib.Test.Diff
{
	[TestFixture]
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

		private class Merger : MergerDefinition<Merger>, IRulesNotFoundFallback
		{
			private Merger()
			{
				Define<Sample>();
			}

			TAlgorithmRules IRulesNotFoundFallback.RulesNotFoundFallback<TAlgorithmRules, TType>(IMergerRulesLocator rulesLocator)
			{
				if (typeof(TAlgorithmRules) == typeof(IDiffAlgorithmRules) && typeof(TType) == typeof(AnotherSample))
					return (TAlgorithmRules)(object)(new CustomDiffAlgorithmRules<TType>());
				return null;
			}
		}

		[Test]
		public void TestInt()
		{
			const string diff =
				"-5\r\n" +
				"+0";

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
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

		[Test]
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

		[Test]
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

		[Test]
		public void TestCustomGuess()
		{
			const string diff = "";

			var @base = new AnotherSample { AnotherValue = "one" };
			var changed = new AnotherSample { AnotherValue = "two" };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(0, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}

		[Test]
		public void TestIEnumerable()
		{
			const string diff =
				"+2:3";
			IEnumerable<int> @base = new[] { 1, 2 };
			IEnumerable<int> changed = new[] { 1, 2, 3 };

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(1, ret.Count);
			Assert.AreEqual(diff, ret.ToString());
		}
	}
}
