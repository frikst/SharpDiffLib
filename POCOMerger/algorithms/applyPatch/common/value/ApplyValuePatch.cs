using System;
using System.Collections.Generic;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.type;
using POCOMerger.implementation;

namespace POCOMerger.algorithms.applyPatch.common.value
{
	internal class ApplyValuePatch<TType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Dictionary<Type, IApplyPatchAlgorithm> aTypes;

		public ApplyValuePatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aTypes = new Dictionary<Type, IApplyPatchAlgorithm>();
		}

		#region Implementation of IApplyPatchAlgorithm

		public object Apply(object source, IDiff patch)
		{
			return this.Apply((TType) source, (IDiff<TType>) patch);
		}

		#endregion

		#region Implementation of IApplyPatchAlgorithm<TType>

		public TType Apply(TType source, IDiff<TType> patch)
		{
			foreach (IDiffValue item in patch)
			{
				if (item is IDiffItemReplaced<TType>)
					return ((IDiffItemReplaced<TType>) item).NewValue;
				if (item is IDiffItemChanged)
				{
					Type sourceType = source.GetType();
					IDiff diff = ((IDiffItemChanged)item).ValueDiff;
					IApplyPatchAlgorithm algorithm;

					if (!this.aTypes.TryGetValue(sourceType, out algorithm))
						algorithm = this.aMergerImplementation.Partial.GetApplyPatchAlgorithm(sourceType);

					return (TType) algorithm.Apply(source, diff);

				}
				else
					throw new InvalidOperationException();
			}

			return source;
		}

		#endregion
	}
}
