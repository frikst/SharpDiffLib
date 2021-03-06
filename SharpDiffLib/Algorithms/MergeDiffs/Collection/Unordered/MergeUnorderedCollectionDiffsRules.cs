﻿using System;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Unordered
{
	public class MergeUnorderedCollectionDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			Property idProperty = GeneralRulesHelper.GetIdProperty(this.MergerImplementation, Class<TType>.EnumerableParam);

			if (idProperty == null)
				return (IMergeDiffsAlgorithm<TType>)Activator.CreateInstance(
					typeof(MergeUnorderedCollectionDiffs<,,>).MakeGenericType(
						typeof(TType),
						Class<TType>.EnumerableParam,
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
			else
				return (IMergeDiffsAlgorithm<TType>)Activator.CreateInstance(
					typeof(MergeUnorderedCollectionDiffs<,,>).MakeGenericType(
						typeof(TType),
						idProperty.Type,
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
		}

		#endregion
	}
}
