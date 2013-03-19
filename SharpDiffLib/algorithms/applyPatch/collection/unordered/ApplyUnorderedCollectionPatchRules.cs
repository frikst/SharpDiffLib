using System;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.applyPatch.collection.unordered
{
	/// <summary>
	/// Rules for unordered collection patch application algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which are rules defined.</typeparam>
	public class ApplyUnorderedCollectionPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

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
