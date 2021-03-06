﻿using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Algorithms.Diff.Collection.Unordered
{
	/// <summary>
	/// Rules for unordered collection diff algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public class UnorderedCollectionDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			Property idProperty = GeneralRulesHelper.GetIdProperty(this.MergerImplementation, Class<TType>.EnumerableParam);

			if (idProperty == null)
				return (IDiffAlgorithm<TType>) Activator.CreateInstance(
					typeof(UnorderedCollectionDiff<,>).MakeGenericType(
						typeof(TType),
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
			else
				return (IDiffAlgorithm<TType>) Activator.CreateInstance(
					typeof(UnorderedCollectionWithIdDiff<,,>).MakeGenericType(
						typeof(TType),
						idProperty.Type,
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffUnorderedCollectionItem);
			yield return typeof(IDiffUnorderedCollectionItemWithID);
			yield return typeof(IDiffItemAdded);
			yield return typeof(IDiffItemRemoved);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		#endregion
	}
}
