using System.Collections.Generic;
using System.Linq;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.type;

namespace SharpDiffLib.algorithms.mergeDiffs.collection.ordered
{
	internal class AutoindexedResult
	{
		private readonly List<IDiffItem> aResult;
		private int aIndexDeltaRet;

		public AutoindexedResult(int capacity)
		{
			this.aResult = new List<IDiffItem>(capacity);
			this.aIndexDeltaRet = 0;
		}

		public List<IDiffItem> ToList()
		{
			return this.aResult;
		}

		public void Add(IDiffOrderedCollectionItem item)
		{
			this.aResult.Add(item.CreateWithDelta(this.aIndexDeltaRet));

			if (item is IDiffItemRemoved)
				this.aIndexDeltaRet--;
		}

		public void Add(IDiffItemConflicted item)
		{
			this.aResult.Add(item);

			if (item.Left.OfType<IDiffItemRemoved>().Any() || item.Right.OfType<IDiffItemRemoved>().Any())
				this.aIndexDeltaRet--;
		}

		public void AddRange(IEnumerable<IDiffOrderedCollectionItem> items)
		{
			int localDelta = 0;

			foreach (IDiffOrderedCollectionItem item in items)
			{
				this.aResult.Add(item.CreateWithDelta(this.aIndexDeltaRet));

				if (item is IDiffItemRemoved)
					localDelta++;
			}

			this.aIndexDeltaRet -= localDelta;
		}
	}
}
