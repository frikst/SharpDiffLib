using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using POCOMerger.definition;
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
		private readonly MergerImplementation aMergerImplementation;
		private IEqualityComparer<TItemType> aEqualityComparer;
		private IDiffAlgorithm<TItemType> aItemDiff;
		private Property aIDProperty;

		public UnorderedCollectionDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;

			this.aIDProperty = GeneralRulesHelper<TItemType>.GetIdProperty(mergerImplementation);
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			if (this.aEqualityComparer == null)
			{
				Type idType = typeof(object);
				if (this.aIDProperty != null)
					idType = this.aIDProperty.Type;

				MethodInfo createEqualityComparer = this.GetType().GetMethod("CreateEqualityComparer", BindingFlags.Instance | BindingFlags.NonPublic);
				this.aEqualityComparer = (IEqualityComparer<TItemType>) createEqualityComparer.MakeGenericMethod(idType).Invoke(this, null);

				this.aItemDiff = this.aMergerImplementation.Partial.GetDiffAlgorithm<TItemType>();
			}

			return this.ComputeInternal((IEnumerable<TItemType>) @base, (IEnumerable<TItemType>) changed);
		}

		#endregion

		private IEqualityComparer<TItemType> CreateEqualityComparer<TIdPropertyType>()
		{
			if (this.aIDProperty == null)
				return EqualityComparer<TItemType>.Default;
			else
			{
				ParameterExpression obj = Expression.Parameter(typeof(TItemType), "obj");

				Expression<Func<TItemType, TIdPropertyType>> propertyGetter =
					Expression.Lambda<Func<TItemType, TIdPropertyType>>(
						Expression.Property(obj, this.aIDProperty.ReflectionPropertyInfo),
						obj
					);

				return new PropertyEqualityComparer<TItemType, TIdPropertyType>(propertyGetter.Compile());
			}
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
