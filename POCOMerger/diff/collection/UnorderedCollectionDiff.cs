using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;
using POCOMerger.implementation;
using POCOMerger.@internal;

namespace POCOMerger.diff.collection
{
	internal class UnorderedCollectionDiff<TType, TItemType> : IDiffAlgorithm<TType>
	{
		private IEqualityComparer<TItemType> aEqualityComparer;

		public UnorderedCollectionDiff(MergerImplementation mergerImplementation)
		{
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aEqualityComparer == null)
				this.aEqualityComparer = this.CreateEqualityComparer();

			return this.ComputeInternal((IEnumerable<TItemType>) @base, (IEnumerable<TItemType>) changed);
		}

		#endregion

		private IEqualityComparer<TItemType> CreateEqualityComparer()
		{
			return EqualityComparer<TItemType>.Default;
		}

		private IDiff<TType> ComputeInternal(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			HashSet<TItemType> changedSet = new HashSet<TItemType>(changed, this.aEqualityComparer);

			List<IDiffItem> ret = new List<IDiffItem>(); // 20 seems to be a good value :)

			foreach (TItemType item in @base)
			{
				if (!changedSet.Remove(item))
					ret.Add(new DiffUnorderedCollectionRemoved<TItemType>(item));
			}

			foreach (TItemType item in changedSet)
			{
				ret.Add(new DiffUnorderedCollectionAdded<TItemType>(item));
			}

			return new Diff<TType>(ret);
		}
	}
}
