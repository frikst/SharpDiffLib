using System;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.definition.rules;
using POCOMerger.fastReflection;

namespace POCOMerger.algorithms.applyPatch.collection.order
{
	public class ApplyOrderedCollectionPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			if (Class<TType>.EnumerableParam == null)
				throw new Exception("Cannot apply patch to non-collection type using ApplyOrderedCollectionPatch");


			return (IApplyPatchAlgorithm<TType>)Activator.CreateInstance(
				typeof(ApplyOrderedCollectionPatch<,>).MakeGenericType(
					typeof(TType),
					Class<TType>.EnumerableParam
				),
				this.MergerImplementation
			);
		}

		#endregion
	}
}
