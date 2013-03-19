using System;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.applyPatch.collection.keyValue
{
	/// <summary>
	/// Rules for key value collection patch application algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which are rules defined.</typeparam>
	public class ApplyKeyValueCollectionPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			if (Class<TType>.KeyValueParams == null)
				throw new Exception("Cannot compare non-collection type with OrderedCollectionDiff");

			return (IApplyPatchAlgorithm<TType>)Activator.CreateInstance(
				typeof(ApplyKeyValueCollectionPatch<,,>).MakeGenericType(
					typeof(TType),
					Class<TType>.KeyValueParams[0],
					Class<TType>.KeyValueParams[1]
				),
				this.MergerImplementation
			);
		}

		#endregion
	}
}
