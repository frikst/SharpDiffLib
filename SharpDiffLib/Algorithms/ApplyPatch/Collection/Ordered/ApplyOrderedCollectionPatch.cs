using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.Ordered
{
	internal class ApplyOrderedCollectionPatch<TType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private Func<List<TItemType>, TType> aConvertor;
		private IApplyPatchAlgorithm<TItemType> aApplyItemDiff;

		public ApplyOrderedCollectionPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aConvertor = null;
			this.aApplyItemDiff = null;
		}

		#region Implementation of IApplyPatchAlgorithm

		public object Apply(object source, IDiff patch)
		{
			return this.Apply((TType)source, (IDiff<TType>)patch);
		}

		#endregion

		#region Implementation of IApplyPatchAlgorithm<TType>

		public TType Apply(TType source, IDiff<TType> patch)
		{
			if (this.aConvertor == null)
			{
				this.aConvertor = this.CompileConvertor();
				this.aApplyItemDiff = this.aMergerImplementation.Partial.Algorithms.GetApplyPatchAlgorithm<TItemType>();
			}

			return this.ApplyInternal((IEnumerable<TItemType>)source, patch);
		}

		#endregion

		private Func<List<TItemType>, TType> CompileConvertor()
		{
			if (typeof(TType).IsGenericType
							&& (typeof(TType).GetGenericTypeDefinition() == typeof(List<>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(IEnumerable<>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(ICollection<>)))
				return x => (TType)(object)x;

			if (typeof(TType).IsArray)
				return x => (TType)(object)x.ToArray();

			ParameterExpression from = Expression.Parameter(typeof(List<TItemType>), "from");

			Expression<Func<List<TItemType>, TType>> convertor = Expression.Lambda<Func<List<TItemType>, TType>>(
				Expression.New(
					typeof(TType).GetConstructor(new[] { typeof(IEnumerable<TItemType>) }),
					Expression.Convert(from, typeof(IEnumerable<TItemType>))
				),
				from
			);

			return convertor.Compile();
		}

		private TType ApplyInternal(IEnumerable<TItemType> source, IDiff<TType> patch)
		{
			List<TItemType> ret = new List<TItemType>();

			using (IEnumerator<TItemType> enumerator = source.GetEnumerator())
			{
				bool lastMoveNext = enumerator.MoveNext();
				int currentIndex = 0;

				foreach (var item in patch.Cast<IDiffOrderedCollectionItem>())
				{
					while (currentIndex < item.ItemIndex)
					{
						ret.Add(enumerator.Current);
						lastMoveNext = enumerator.MoveNext();
						currentIndex++;
					}

					switch (item)
					{
						case IDiffItemAdded<TItemType> itemAdded:
							ret.Add(itemAdded.NewValue);
							break;
						case IDiffItemChanged<TItemType> itemChanged:
							ret.Add(this.aApplyItemDiff.Apply(enumerator.Current, itemChanged.ValueDiff));
							lastMoveNext = enumerator.MoveNext();
							currentIndex++;
							break;
						case IDiffItemRemoved<TItemType> _:
							lastMoveNext = enumerator.MoveNext();
							currentIndex++;
							break;
						case IDiffItemReplaced<TItemType> itemReplaced:
							lastMoveNext = enumerator.MoveNext();
							ret.Add(itemReplaced.NewValue);
							currentIndex++;
							break;
						default:
							throw new InvalidOperationException();
					}
				}

				while (lastMoveNext)
				{
					ret.Add(enumerator.Current);
					lastMoveNext = enumerator.MoveNext();
				}
			}

			return this.aConvertor(ret);
		}
	}
}
