using System;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;
using POCOMerger.fastReflection;

namespace POCOMerger.algorithms.applyPatch.collection.unordered
{
	public class ApplyUnorderedCollectionPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		public IApplyPatchAlgorithm<TType> GetAlgorithm<TType>()
		{
			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			Property idProperty = GeneralRulesHelper.GetIdProperty(this.MergerImplementation, Class<TType>.EnumerableParam);

			if (idProperty == null)
				return (IApplyPatchAlgorithm<TType>)Activator.CreateInstance(
					typeof(ApplyUnorderedCollectionPatch<,>).MakeGenericType(
						typeof(TType),
						Class<TType>.EnumerableParam
					),
					this.MergerImplementation
				);
			else
				return (IApplyPatchAlgorithm<TType>)Activator.CreateInstance(
					typeof(ApplyUnorderedCollectionWithIdPatch<,,>).MakeGenericType(
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
