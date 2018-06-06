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
				switch (item)
				{
					case IDiffItemReplaced<TType> itemReplaced:
						return itemReplaced.NewValue;
					case IDiffItemChanged itemChanged:
						Type sourceType = source.GetType();
						IDiff diff = itemChanged.ValueDiff;
						IApplyPatchAlgorithm algorithm;

						if (!this.aTypes.TryGetValue(sourceType, out algorithm))
							algorithm = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm(sourceType,this.aRules);

						return (TType) algorithm.Apply(source, diff);
					default:
						throw new InvalidOperationException();
				}
			}

			return source;
		}

		#endregion
	}
}
