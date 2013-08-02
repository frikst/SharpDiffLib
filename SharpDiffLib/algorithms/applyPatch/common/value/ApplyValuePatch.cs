﻿using System;
using System.Collections.Generic;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.type;
using SharpDiffLib.implementation;

namespace SharpDiffLib.algorithms.applyPatch.common.value
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
