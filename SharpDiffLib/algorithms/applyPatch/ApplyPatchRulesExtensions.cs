using System;
using SharpDiffLib.algorithms.applyPatch.collection.keyValue;
using SharpDiffLib.algorithms.applyPatch.collection.order;
using SharpDiffLib.algorithms.applyPatch.collection.unordered;
using SharpDiffLib.algorithms.applyPatch.common.@class;
using SharpDiffLib.algorithms.applyPatch.common.value;
using SharpDiffLib.definition;

namespace SharpDiffLib.algorithms.applyPatch
{
	/// <summary>
	/// Extension methods for simpler patch application algorithm rules definition using fluent syntax.
	/// </summary>
	public static class ApplyPatchRulesExtensions
	{
		/// <summary>
		/// Define the key value collection patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyKeyValueCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyKeyValueCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the key value collection patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyKeyValueCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyKeyValueCollectionPatchRules<TClass>>();
		}

		/// <summary>
		/// Define the ordered collection patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyOrderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyOrderedCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the ordered collection patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyOrderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyOrderedCollectionPatchRules<TClass>>();
		}

		/// <summary>
		/// Define the unordered collection patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyUnorderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyUnorderedCollectionPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the unordered collection patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyUnorderedCollectionPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyUnorderedCollectionPatchRules<TClass>>();
		}

		/// <summary>
		/// Define the class patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyClassPatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyClassPatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the class patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyClassPatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyClassPatchRules<TClass>>();
		}

		/// <summary>
		/// Define the simple value patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyValuePatchRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ApplyValuePatchRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the simple value patch application algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ApplyValuePatchRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ApplyValuePatchRules<TClass>>();
		}
	}
}
