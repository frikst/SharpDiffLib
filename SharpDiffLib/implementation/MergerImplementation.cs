using System;
using System.Collections.Generic;
using System.Linq;
using SharpDiffLib.algorithms.applyPatch.@base;
using SharpDiffLib.algorithms.applyPatch.collection.keyValue;
using SharpDiffLib.algorithms.applyPatch.collection.order;
using SharpDiffLib.algorithms.applyPatch.collection.unordered;
using SharpDiffLib.algorithms.applyPatch.common.@class;
using SharpDiffLib.algorithms.applyPatch.common.value;
using SharpDiffLib.algorithms.diff.@base;
using SharpDiffLib.algorithms.diff.collection.keyValue;
using SharpDiffLib.algorithms.diff.collection.ordered;
using SharpDiffLib.algorithms.diff.collection.unordered;
using SharpDiffLib.algorithms.diff.common.@class;
using SharpDiffLib.algorithms.diff.common.value;
using SharpDiffLib.algorithms.mergeDiffs.@base;
using SharpDiffLib.algorithms.mergeDiffs.collection.keyValue;
using SharpDiffLib.algorithms.mergeDiffs.collection.ordered;
using SharpDiffLib.algorithms.mergeDiffs.collection.unordered;
using SharpDiffLib.algorithms.mergeDiffs.common.@class;
using SharpDiffLib.algorithms.mergeDiffs.common.value;
using SharpDiffLib.algorithms.resolveConflicts.@base;
using SharpDiffLib.algorithms.resolveConflicts.common.dontResolve;
using SharpDiffLib.@base;
using SharpDiffLib.conflictManagement;
using SharpDiffLib.definition;
using SharpDiffLib.definition.rules;
using SharpDiffLib.diffResult.@base;

namespace SharpDiffLib.implementation
{
	public class MergerImplementation
	{
		private class MergerRulesLocator : IMergerRulesLocator
		{
			private readonly MergerImplementation aMergerImplementation;

			public MergerRulesLocator(MergerImplementation mergerImplementation)
			{
				this.aMergerImplementation = mergerImplementation;
			}

			#region Implementation of IMergerRulesLocator

			public TRules GetMergerRulesFor<TRules>(Type type)
				where TRules : class, IAlgorithmRules
			{
				return this.aMergerImplementation.GetMergerRulesFor<TRules>(type);
			}

			public TRules GetMergerRulesForWithDefault<TRules>(Type type)
				where TRules : class, IAlgorithmRules
			{
				return this.aMergerImplementation.GetMergerRulesForWithDefault<TRules>(type);
			}

			public TRules GuessRules<TRules>(Type type)
				where TRules : class, IAlgorithmRules
			{
				return this.aMergerImplementation.GuessRules<TRules>(type);
			}

			#endregion
		}

		private readonly IEnumerable<IClassMergerDefinition> aDefinitions;
		private readonly Func<Type, Type, IMergerRulesLocator, IAlgorithmRules> aRulesNotFoundFallback;

		internal MergerImplementation(IEnumerable<IClassMergerDefinition> definitions, Func<Type, Type, IMergerRulesLocator, IAlgorithmRules> rulesNotFoundFallback)
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
			ConflictContainer conflicts = new ConflictContainer();

			IDiff<TType> patch = this.Partial.MergeDiffs(
				this.Partial.Diff(@base, left),
				this.Partial.Diff(@base, right),
				conflicts
			);

			if (conflicts.HasConflicts)
			{
				ConflictResolver<TType> resolver = new ConflictResolver<TType>(patch, conflicts);

				this.Partial.ResolveConflicts(resolver);

				patch = resolver.Finish();
			}

			return this.Partial.ApplyPatch(@base, patch);
		}

		internal TRules GetMergerRulesFor<TRules>(Type type)
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

			ret = this.aRulesNotFoundFallback(typeof(TRules), type, new MergerRulesLocator(this)) as TRules;

			if (ret == null)
				ret = this.GuessRules<TRules>(type);

			if (ret != null)
				ret.Initialize(this);

			return ret;
		}

		private TRules GuessRules<TRules>(Type type)
			where TRules : class, IAlgorithmRules
		{
			bool implementsIEnumerable = false;
			bool implementsIEnumerableKeyValue = false;
			bool implementsISet = false;

			Type ret = null;

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

			if (typeof(IDiffAlgorithmRules).IsAssignableFrom(typeof(TRules)))
			{
				if (type.IsValueType || type == typeof(string))
					ret = typeof(ValueDiffRules<>);
				else if (implementsISet)
					ret = typeof(UnorderedCollectionDiffRules<>);
				else if (implementsIEnumerableKeyValue)
					ret = typeof(KeyValueCollectionDiffRules<>);
				else if (implementsIEnumerable)
					ret = typeof(OrderedCollectionDiffRules<>);
				else if (this.GetMergerAnyDefinition(type) != null)
					ret = typeof(ClassDiffRules<>);
				else
					ret = typeof(ValueDiffRules<>);
			}
			else if (typeof(IApplyPatchAlgorithmRules).IsAssignableFrom(typeof(TRules)))
			{
				if (type.IsValueType || type == typeof(string))
					ret = typeof(ApplyValuePatchRules<>);
				else if (implementsISet)
					ret = typeof(ApplyUnorderedCollectionPatchRules<>);
				else if (implementsIEnumerableKeyValue)
					ret = typeof(ApplyKeyValueCollectionPatchRules<>);
				else if (implementsIEnumerable)
					ret = typeof(ApplyOrderedCollectionPatchRules<>);
				else if (this.GetMergerAnyDefinition(type) != null)
					ret = typeof(ApplyClassPatchRules<>);
				else
					ret = typeof(ApplyValuePatchRules<>);
			}
			else if (typeof(IResolveConflictsAlgorithmRules).IsAssignableFrom(typeof(TRules)))
			{
				ret = typeof(DontResolveRules<>);
			}
			else if (typeof(IMergeDiffsAlgorithmRules).IsAssignableFrom(typeof(TRules)))
			{
				if (type.IsValueType || type == typeof(string))
					ret = typeof(MergeValueDiffsRules<>);
				else if (implementsISet)
					ret = typeof(MergeUnorderedCollectionDiffsRules<>);
				else if (implementsIEnumerableKeyValue)
					ret = typeof(MergeKeyValueCollectionDiffsRules<>);
				else if (implementsIEnumerable)
					ret = typeof(MergeOrderedCollectionDiffsRules<>);
				else if (this.GetMergerAnyDefinition(type) != null)
					ret = typeof(MergeClassDiffsRules<>);
				else
					ret = typeof(MergeValueDiffsRules<>);
			}

			if (ret != null)
				return (TRules) Activator.CreateInstance(ret.MakeGenericType(type));
			else
				return null;
		}
	}
}