using System;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.definition.rules;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.algorithms.applyPatch.collection.order
{
	/// <summary>
	/// Rules for ordered collection patch application algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public class ApplyOrderedCollectionPatchRules<TDefinedFor> : BaseRules<TDefinedFor>, IApplyPatchAlgorithmRules<TDefinedFor>
	{
		#region Implementation of IApplyPatchAlgorithmRules

		IApplyPatchAlgorithm<TType> IApplyPatchAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

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
