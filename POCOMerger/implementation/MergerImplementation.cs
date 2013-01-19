using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.definition;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diff.collection;
using POCOMerger.diff.collection.keyValue;
using POCOMerger.diff.collection.ordered;
using POCOMerger.diff.collection.unordered;
using POCOMerger.diff.common;
using POCOMerger.diff.common.@class;
using POCOMerger.diff.common.value;

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
			return this.Partial.ApplyPatch(
				@base,
				this.Partial.ResolveConflicts(
					this.Partial.Merge(
						this.Partial.Diff(@base, left),
						this.Partial.Diff(@base, right)
					)
				)
			);
		}

		public TRules GetMergerRulesFor<TRules>(Type type)
			where TRules : class, IAlgorithmRules
		{
			for (Type tmp = type; tmp != null; tmp = tmp.BaseType)
			{
				foreach (IClassMergerDefinition definition in this.aDefinitions.Where(mergerDefinition => mergerDefinition.DefinedFor == tmp))
				{
					TRules rules = definition.GetRules<TRules>();

					if (rules != null && (rules.IsInheritable || tmp == type))
						return rules;
				}
			}

			return null;
		}

		private IClassMergerDefinition GetMergerAnyDefinition(Type type)
		{
			for (Type tmp = type; tmp != null; tmp = tmp.BaseType)
			{
				foreach (IClassMergerDefinition definition in this.aDefinitions.Where(mergerDefinition => mergerDefinition.DefinedFor == tmp))
				{
					bool anyInheritable = definition.GetAllRules<IAlgorithmRules>().Any(x => x.IsInheritable);

					if (anyInheritable || tmp == type)
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

			if (this.aRulesNotFoundFallback != null)
				ret = this.aRulesNotFoundFallback(typeof(TRules), type) as TRules;

			if (ret == null)
			{


				// guess rules for the given type

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

				if (typeof(IDiffAlgorithmRules).IsAssignableFrom(typeof(TRules)))
				{
					if (implementsISet)
						ret = (TRules) (object) new UnorderedCollectionDiffRules();
					else if (implementsIEnumerableKeyValue)
						ret = (TRules)(object)new KeyValueCollectionDiffRules();
					else if (implementsIEnumerable)
						ret = (TRules)(object)new OrderedCollectionDiffRules();
					else if (this.GetMergerAnyDefinition(type) != null)
						ret = (TRules)(object)new ClassDiffRules();
					else
						ret = (TRules)(object)new ValueDiffRules();
				}
			}

			if (ret != null)
				ret.Initialize(this);

			return ret;
		}
	}
}