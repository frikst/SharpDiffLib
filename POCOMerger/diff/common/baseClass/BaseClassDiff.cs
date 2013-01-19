using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.implementation;

namespace POCOMerger.diff.common.baseClass
{
	internal class BaseClassDiff<TType> : IDiffAlgorithm<TType>
	{
		private readonly MergerImplementation aMergerImplementation;
		private readonly Dictionary<Type, IDiffAlgorithm> aTypes;

		public BaseClassDiff(MergerImplementation mergerImplementation)
		{
			this.aMergerImplementation = mergerImplementation;
			this.aTypes = new Dictionary<Type, IDiffAlgorithm>();
		}

		#region Implementation of IDiffAlgorithm<TType>

		public IDiff<TType> Compute(TType @base, TType changed)
		{
			Type baseType = @base.GetType();
			Type changedType = changed.GetType();

			if (baseType != changedType)
			{
				return new Diff<TType>(
					new List<IDiffItem>(1)
					{
						new DiffValueReplaced<TType>(@base, changed)
					}
				);
			}
			else
			{
				IDiffAlgorithm algorithm;

				if (!this.aTypes.TryGetValue(baseType, out algorithm))
					algorithm = this.aMergerImplementation.Partial.GetDiffAlgorithm(baseType);

				return (IDiff<TType>) algorithm.Compute(@base, changed);
			}
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
	}
}
