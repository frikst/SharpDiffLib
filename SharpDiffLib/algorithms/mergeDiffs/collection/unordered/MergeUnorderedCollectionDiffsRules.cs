using System;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.mergeDiffs.collection.unordered
{
	public class MergeUnorderedCollectionDiffsRules<TDefinedFor> : BaseRules<TDefinedFor>, IMergeDiffsAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IMergeDiffsAlgorithmRules

		IMergeDiffsAlgorithm<TType> IMergeDiffsAlgorithmRules.GetAlgorithm<TType>()
		{
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
