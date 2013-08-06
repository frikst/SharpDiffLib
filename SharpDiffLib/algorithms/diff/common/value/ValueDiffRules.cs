﻿using System;
using System.Collections.Generic;
using SharpDiffLib.algorithms.diff.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.type;

namespace SharpDiffLib.algorithms.diff.common.value
{
	/// <summary>
	/// Rules for simple value diff algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public class ValueDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new ValueDiff<TType>(this.MergerImplementation);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffValue);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}