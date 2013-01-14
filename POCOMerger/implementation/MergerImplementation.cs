using System;
using System.Collections.Generic;
using System.Linq;
using POCOMerger.definition;

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

		internal IClassMergerDefinition GetMergerFor(Type type)
		{
			return this.aDefinitions.FirstOrDefault(mergerDefinition => mergerDefinition.DefinedFor == type);
		}
	}
}