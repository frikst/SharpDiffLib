using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using POCOMerger.algorithms.mergeDiffs.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.mergeDiffs.common.@class
{
	internal class MergeClassDiffs<TType> : IMergeDiffsAlgorithm<TType>
	{
		private struct CompiledReturn
		{
			public List<IDiffItem> aDiff;
			public bool aHadConflicts;
		}

		private readonly MergerImplementation aMergerImplementation;
		private Func<IDiff<TType>, IDiff<TType>, CompiledReturn> aCompiled;

		public MergeClassDiffs(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
		}

		#region Implementation of IMergeDiffsAlgorithm<TType>

		public IDiff<TType> MergeDiffs(IDiff<TType> left, IDiff<TType> right, out bool hadConflicts)
		{
			if (this.aCompiled == null)
			{
				var compiled = this.Compile();

				this.aCompiled = compiled.Compile();
			}

			var ret = this.aCompiled(left, right);

			hadConflicts = ret.aHadConflicts;
			return new Diff<TType>(ret.aDiff);

		}

		#endregion

		private Expression<Func<IDiff<TType>, IDiff<TType>, CompiledReturn>> Compile()
		{
			throw new NotImplementedException();
		}
	}
}
