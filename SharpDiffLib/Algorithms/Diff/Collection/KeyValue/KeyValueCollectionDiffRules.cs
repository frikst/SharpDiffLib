﻿using System;
using System.Collections.Generic;
using SharpDiffLib.algorithms.diff.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.type;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.diff.collection.keyValue
{
	/// <summary>
	/// Rules for key value collection diff algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public class KeyValueCollectionDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			if (Class<TType>.KeyValueParams == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			return (IDiffAlgorithm<TType>)Activator.CreateInstance(
				typeof(KeyValueCollectionDiff<,,>).MakeGenericType(
					typeof(TType),
					Class<TType>.KeyValueParams[0],
					Class<TType>.KeyValueParams[1]
				),
				this.MergerImplementation
			);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffKeyValueCollectionItem);
			yield return typeof(IDiffItemAdded);
			yield return typeof(IDiffItemRemoved);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}