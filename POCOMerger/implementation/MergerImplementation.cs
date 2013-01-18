using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.definition;
using POCOMerger.definition.rules;

namespace POCOMerger.implementation
{
	public class MergerImplementation
	{
		private readonly IEnumerable<IClassMergerDefinition> aDefinitions;

		internal MergerImplementation(IEnumerable<IClassMergerDefinition> definitions)
		{
			this.aDefinitions = definitions.ToList();

			foreach (IClassMergerDefinition definition in definitions)
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

		internal TRules GetMergerRulesFor<TRules>(Type type)
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
	}
}