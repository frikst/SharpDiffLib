using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.ApplyPatch.Base;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.Implementation;

namespace KST.SharpDiffLib.Algorithms.ApplyPatch.Collection.Unordered
{
	internal class ApplyUnorderedCollectionPatch<TType, TItemType> : IApplyPatchAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private IEqualityComparer<TItemType> aItemComparer;
		private Func<HashSet<TItemType>, TType> aConvertor;

		public ApplyUnorderedCollectionPatch(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
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
			if (this.aItemComparer == null)
			{
				this.aItemComparer = EqualityComparer<TItemType>.Default;
				this.aConvertor = this.CompileConvertor();
			}

			return this.ApplyInternal((IEnumerable<TItemType>)source, patch);
		}

		#endregion

		private Func<HashSet<TItemType>, TType> CompileConvertor()
		{
			if (typeof(TType).IsGenericType
							&& (typeof(TType).GetGenericTypeDefinition() == typeof(HashSet<>)
								|| typeof(TType).GetGenericTypeDefinition() == typeof(ISet<>)))
				return x => (TType)(object)x;

			if (typeof(TType).IsArray)
				return x => (TType)(object)x.ToArray();

			ParameterExpression from = Expression.Parameter(typeof(HashSet<TItemType>), "from");

			Expression<Func<HashSet<TItemType>, TType>> convertor = Expression.Lambda<Func<HashSet<TItemType>, TType>>(
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
			HashSet<TItemType> ret = new HashSet<TItemType>(source, this.aItemComparer);

			foreach (var item in patch.Cast<IDiffUnorderedCollectionItem>())
			{
				switch (item)
				{
					case IDiffItemAdded<TItemType> itemAdded:
						ret.Add(itemAdded.NewValue);
						break;
					case IDiffItemRemoved<TItemType> itemRemoved:
						ret.Remove(itemRemoved.OldValue);
						break;
					case IDiffItemReplaced<TItemType> itemReplaced:
						ret.Remove(itemReplaced.OldValue);
						ret.Add(itemReplaced.NewValue);
						break;
					default:
						throw new InvalidOperationException();
				}
			}

			return this.aConvertor(ret);
		}
	}
}
