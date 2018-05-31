using System;
using KST.SharpDiffLib.Algorithms.Diff.Collection.KeyValue;
using KST.SharpDiffLib.Algorithms.Diff.Collection.Ordered;
using KST.SharpDiffLib.Algorithms.Diff.Collection.Unordered;
using KST.SharpDiffLib.Algorithms.Diff.Common.BaseClass;
using KST.SharpDiffLib.Algorithms.Diff.Common.Class;
using KST.SharpDiffLib.Algorithms.Diff.Common.Value;
using KST.SharpDiffLib.Definition;

namespace KST.SharpDiffLib.Algorithms.Diff
{
	/// <summary>
	/// Extension methods for simpler diff algorithm rules definition using fluent syntax.
	/// </summary>
	public static class DiffRulesExtensions
	{
		/// <summary>
		/// Define the key value collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> KeyValueCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<KeyValueCollectionDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the key value collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> KeyValueCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<KeyValueCollectionDiffRules<TClass>>();
		}

		/// <summary>
		/// Define the ordered collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> OrderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<OrderedCollectionDiffRules<TClass>> func)
		{
			return  definition.Rules(func);
		}

		/// <summary>
		/// Define the key value collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> OrderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<OrderedCollectionDiffRules<TClass>>();
		}

		/// <summary>
		/// Define the unordered collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> UnorderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<UnorderedCollectionDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the unordered collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> UnorderedCollectionDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<UnorderedCollectionDiffRules<TClass>>();
		}

		/// <summary>
		/// Define the class collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ClassDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the class collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ClassDiffRules<TClass>>();
		}

		/// <summary>
		/// Define the simple value collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ValueDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<ValueDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the simple value collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> ValueDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<ValueDiffRules<TClass>>();
		}

		/// <summary>
		/// Define the base class collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <param name="func">Definition function</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> BaseClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition, Action<BaseClassDiffRules<TClass>> func)
		{
			return definition.Rules(func);
		}

		/// <summary>
		/// Define the base class collection diff algorithm rules.
		/// </summary>
		/// <typeparam name="TClass">Type for which the rules should be defined.</typeparam>
		/// <param name="definition">Merger definition for the given type.</param>
		/// <returns>Merger definition for the later use in fluent definition.</returns>
		public static ClassMergerDefinition<TClass> BaseClassDiffRules<TClass>(this ClassMergerDefinition<TClass> definition)
		{
			return definition.Rules<BaseClassDiffRules<TClass>>();
		}
	}
}
