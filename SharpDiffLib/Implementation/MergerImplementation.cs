﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.KeyValue;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.Ordered;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.Unordered;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Common.Class;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Common.Value;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Algorithms.Diff.Collection.KeyValue;
using KST.SharpDiffLib.Algorithms.Diff.Collection.Ordered;
using KST.SharpDiffLib.Algorithms.Diff.Collection.Unordered;
using KST.SharpDiffLib.Algorithms.Diff.Common.Class;
using KST.SharpDiffLib.Algorithms.Diff.Common.Value;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Base;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.KeyValue;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Ordered;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Collection.Unordered;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Common.Class;
using KST.SharpDiffLib.Algorithms.MergeDiffs.Common.Value;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Base;
using KST.SharpDiffLib.Algorithms.ResolveConflicts.Common.DontResolve;
using KST.SharpDiffLib.Base;
using KST.SharpDiffLib.ConflictManagement;
using KST.SharpDiffLib.Definition;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.Internal.Members;

namespace KST.SharpDiffLib.Implementation
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
				return this.aMergerImplementation.GetMergerRulesFor<TRules>(type, null);
			}

			public TRules GetMergerRulesForWithDefault<TRules>(Type type)
				where TRules : class, IAlgorithmRules
			{
				return this.aMergerImplementation.GetMergerRulesForWithDefault<TRules>(type, null);
			}

			public TRules GuessRules<TRules>(Type type)
				where TRules : class, IAlgorithmRules
			{
				return this.aMergerImplementation.GuessRules<TRules>(type);
			}

			#endregion
		}

		private readonly IEnumerable<IClassMergerDefinition> aDefinitions;
		private readonly IRulesNotFoundFallback aRulesNotFoundFallback;

		internal MergerImplementation(IEnumerable<IClassMergerDefinition> definitions, IRulesNotFoundFallback rulesNotFoundFallback)
		{
			this.aRulesNotFoundFallback = rulesNotFoundFallback;

			this.aDefinitions = definitions.ToList();

			foreach (IClassMergerDefinition definition in this.aDefinitions)
			{
				definition.Initialize(this);
			}

			this.Partial = new PartialMergerAlgorithms(this);
		}

		public PartialMergerAlgorithms Partial { get; }

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

		internal TRules GetMergerRulesFor<TRules>(Type type, IAlgorithmRules ignore)
			where TRules : class, IAlgorithmRules
		{
			foreach (IClassMergerDefinition definition in this.GetDefinitionFor(type))
			{
				TRules rules = definition.GetRules<TRules>(ignore);

				if (rules != null)
					return rules;
			}

			for (Type tmp = type.BaseType; tmp != null; tmp = tmp.BaseType)
			{
				foreach (IClassMergerDefinition definition in this.GetDefinitionFor(tmp))
				{
					foreach (TRules rules in definition.GetAllRules<TRules>())
					{
						if (rules is IInheritableAlgorithmRules inheritable)
							if (inheritable.IsInheritable)
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
					if (definition.GetAllRules<IAlgorithmRules>().OfType<IInheritableAlgorithmRules>().Any(x => x.IsInheritable))
						return definition;
				}
			}

			return null;
		}

		internal TRules GetMergerRulesForWithDefault<TRules>(Type type, IAlgorithmRules ignore)
			where TRules : class, IAlgorithmRules
		{
			TRules ret = this.GetMergerRulesFor<TRules>(type, ignore);

			if (ret != null)
				return ret;

			if (this.aRulesNotFoundFallback != null)
				ret = RulesNotFoundFallbackMembers.RulesNotFoundFallback(typeof(TRules), type)
					.Invoke(this.aRulesNotFoundFallback, new object[] {new MergerRulesLocator(this)}) as TRules;

			if (ret == null)
				ret = this.GuessRules<TRules>(type);

			ret?.Initialize(this);

			return ret;
		}

		private TRules GuessRules<TRules>(Type type)
			where TRules : class, IAlgorithmRules
		{
			bool implementsIEnumerable = false;
			bool implementsIEnumerableKeyValue = false;
			bool implementsISet = false;

			Type ret = null;

			IEnumerable<Type> interfaces = type.GetInterfaces();

			if (type.IsInterface)
				interfaces = new[] { type }.Concat(interfaces);

			foreach (Type @interface in interfaces)
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