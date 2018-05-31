using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Common.Value
{
	internal class ApplyValuePatch<TType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Dictionary<Type, IApplyPatchAlgorithm> aTypes;
		private readonly IAlgorithmRules aRules;

		public ApplyValuePatch(MergerImplementation mergerImplementation, IAlgorithmRules rules)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aRules = rules;
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
						algorithm = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm(sourceType,this.aRules);

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
