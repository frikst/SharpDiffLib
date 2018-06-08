using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Factory;
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
				return DiffFactory.Create<TType>().Class()
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
			var expected = DiffFactory.Create<int>().Value()
				.Replaced(5, 0)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(5, 0);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIntArray()
		{
			var @base = new[] { 1, 2 };
			var changed = new[] { 1, 2, 3 };

			var expected = DiffFactory.Create<int[]>().Ordered()
				.Added(2, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIntSet()
		{
			var @base = new HashSet<int> { 1, 2 };
			var changed = new HashSet<int> { 1, 2, 3 };

			var expected = DiffFactory.Create<HashSet<int>>().Unordered()
				.Added(3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestDifferentObjects()
		{
			var @base = new Sample { Value = "one" };
			var changed = new Sample { Value = "two" };

			var expected = DiffFactory.Create<Sample>().Class()
				.Replaced(x => x.Value, "one", "two")
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestCustomGuess()
		{
			var @base = new AnotherSample { AnotherValue = "one" };
			var changed = new AnotherSample { AnotherValue = "two" };

			var expected = DiffFactory.Create<AnotherSample>().Class()
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}

		[Test]
		public void TestIEnumerable()
		{
			IEnumerable<int> @base = new[] { 1, 2 };
			IEnumerable<int> changed = new[] { 1, 2, 3 };

			var expected = DiffFactory.Create<int[]>().Ordered()
				.Added(2, 3)
				.MakeDiff();

			var ret = Merger.Instance.Partial.Diff(@base, changed);

			Assert.AreEqual(expected, ret);
		}
	}
}
