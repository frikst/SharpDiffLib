using System;
using System.Collections.Generic;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.Diff.Collection.Unordered
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

		#region Implementation of IDiffAlgorithm

		public bool IsDirect
		{
			get { return false; }
		}

		IDiff IDiffAlgorithm.Compute(object @base, object changed)
		{
			if (!(@base is TType && changed is TType))
				throw new InvalidOperationException("base and changed has to be of type " + typeof(TType).Name);

			return this.Compute((TType)@base, (TType)changed);
		}

		#endregion

		private IEqualityComparer<TItemType> CreateEqualityComparer()
		{
			return EqualityComparer<TItemType>.Default;
		}

		private IDiff<TType> ComputeInternal(IEnumerable<TItemType> @base, IEnumerable<TItemType> changed)
		{
			HashSet<TItemType> changedSet = new HashSet<TItemType>(changed, this.aEqualityComparer);

			List<IDiffItem> ret = new List<IDiffItem>(20); // 20 seems to be a good value :)

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
