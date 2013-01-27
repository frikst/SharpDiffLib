using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.algorithms.applyPatch.@base;
using POCOMerger.algorithms.applyPatch.collection.keyValue;
using POCOMerger.algorithms.applyPatch.collection.order;
using POCOMerger.algorithms.applyPatch.collection.unordered;
using POCOMerger.algorithms.applyPatch.common.@class;
using POCOMerger.algorithms.applyPatch.common.value;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.algorithms.diff.collection.keyValue;
using POCOMerger.algorithms.diff.collection.ordered;
using POCOMerger.algorithms.diff.collection.unordered;
using POCOMerger.algorithms.diff.common.@class;
using POCOMerger.algorithms.diff.common.value;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.@base;

namespace POCOMerger.implementation
{
	public class MergerImplementation
	{
		private readonly IEnumerable<IClassMergerDefinition> aDefinitions;
		private readonly Func<Type, Type, IAlgorithmRules> aRulesNotFoundFallback;

		internal MergerImplementation(IEnumerable<IClassMergerDefinition> definitions, Func<Type, Type, IAlgorithmRules> rulesNotFoundFallback)
		{
			this.aRulesNotFoundFallback = rulesNotFoundFallback;
			this.aDefinitions = definitions.ToList();

			foreach (IClassMergerDefinition definition in this.aDefinitions)
			{
				definition.Initialize(this);
			}

			this.Partial = new PartialMergerAlgorithms(this);
		}

		public PartialMergerAlgorithms Partial { get; private set; }

		public TType Merge<TType>(TType @base, TType left, TType right)
		{
			bool hadConflicts;

			IDiff<TType> patch = this.Partial.MergeDiffs(
				this.Partial.Diff(@base, left),
				this.Partial.Diff(@base, right),
				out hadConflicts
			);

			if (hadConflicts)
				patch = this.Partial.ResolveConflicts(patch);

			return this.Partial.ApplyPatch(@base, patch);
		}

		public TRules GetMergerRulesFor<TRules>(Type type)
			where TRules : class, IAlgorithmRules
		{
			foreach (IClassMergerDefinition definition in this.GetDefinitionFor(type))
			{
				TRules rules = definition.GetRules<TRules>();

				if (rules != null)
					return rules;
			}

			for (Type tmp = type.BaseType; tmp != null; tmp = tmp.BaseType)
			{
				foreach (IClassMergerDefinition definition in this.GetDefinitionFor(tmp))
				{
					foreach (TRules rules in definition.GetAllRules<TRules>())
					{
						if (rules.IsInheritable)
							return rules;
					}
				}
			}

			return null;
		}

		private IEnumerable<IClassMergerDefinition> GetDefinitionFor(Type tested)
		{
			return this.aDefinitions.Where(mergerDefinition => mergerDefinition.DefinedFor == tested);
		}

		private IClassMergerDefinition GetMergerAnyDefinition(Type type)
		{
			foreach (IClassMergerDefinition definition in this.GetDefinitionFor(type))
				return definition;

			for (Type tmp = type.BaseType; tmp != null; tmp = tmp.BaseType)
			{
				foreach (IClassMergerDefinition definition in this.GetDefinitionFor(tmp))
				{
					if (definition.GetAllRules<IAlgorithmRules>().Any(x => x.IsInheritable))
						return definition;
				}
			}

			return null;
		}

		internal TRules GetMergerRulesForWithDefault<TRules>(Type type)
			where TRules : class, IAlgorithmRules
		{
			TRules ret = this.GetMergerRulesFor<TRules>(type);

			if (ret != null)
				return ret;

			ret = this.aRulesNotFoundFallback(typeof(TRules), type) as TRules;

			if (ret == null)
			{
				var rules = this.GuessRules<TRules>(type, typeof(TRules));

				if (rules != null)
					ret = (TRules) Activator.CreateInstance(rules.MakeGenericType(type));
			}

			if (ret != null)
				ret.Initialize(this);

			return ret;
		}

		private Type GuessRules<TRules>(Type type, Type rulesType) where TRules : class, IAlgorithmRules
		{
			bool implementsIEnumerable = false;
			bool implementsIEnumerableKeyValue = false;
			bool implementsISet = false;

			foreach (Type @interface in type.GetInterfaces())
			{
				if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					implementsIEnumerable = true;

					Type param = @interface.GetGenericArguments()[0];

					if (param.IsGenericType && param.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
						implementsIEnumerableKeyValue = true;
				}
				else if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ISet<>))
					implementsISet = true;
			}

			if (typeof(IDiffAlgorithmRules).IsAssignableFrom(rulesType))
			{
				if (type.IsValueType || type == typeof(string))
					return typeof(ValueDiffRules<>);
				else if (implementsISet)
					return typeof(UnorderedCollectionDiffRules<>);
				else if (implementsIEnumerableKeyValue)
					return typeof(KeyValueCollectionDiffRules<>);
				else if (implementsIEnumerable)
					return typeof(OrderedCollectionDiffRules<>);
				else if (this.GetMergerAnyDefinition(type) != null)
					return typeof(ClassDiffRules<>);
				else
					return typeof(ValueDiffRules<>);
			}
			else if (typeof(IApplyPatchAlgorithmRules).IsAssignableFrom(rulesType))
			{
				if (type.IsValueType || type == typeof(string))
					return typeof(ApplyValuePatchRules<>);
				else if (implementsISet)
					return typeof(ApplyUnorderedCollectionPatchRules<>);
				else if (implementsIEnumerableKeyValue)
					return typeof(ApplyKeyValueCollectionPatchRules<>);
				else if (implementsIEnumerable)
					return typeof(ApplyOrderedCollectionPatchRules<>);
				else if (this.GetMergerAnyDefinition(type) != null)
					return typeof(ApplyClassPatchRules<>);
				else
					return typeof(ApplyValuePatchRules<>);
			}
			else
			{
				return null;
			}
		}
	}
}