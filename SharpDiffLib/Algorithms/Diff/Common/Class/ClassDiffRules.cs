using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KST.SharpDiffLib.Algorithms.Diff.Base;
using KST.SharpDiffLib.Definition.Rules;
using KST.SharpDiffLib.DiffResult.Action;
using KST.SharpDiffLib.DiffResult.Type;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.Algorithms.Diff.Common.Class
{
	/// <summary>
	/// Rules for class diff algorithm.
	/// </summary>
	/// <typeparam name="TDefinedFor">Type for which the rules are defined.</typeparam>
	public class ClassDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>, IClassDiffRules, IInheritableAlgorithmRules
	{
		private readonly HashSet<Property> aIgnoreProperties;
		private readonly HashSet<Property> aAlwaysIncludedProperties;

		public ClassDiffRules()
		{
			this.aIgnoreProperties = new HashSet<Property>();
			this.aAlwaysIncludedProperties = new HashSet<Property>();
		}

		public ClassDiffRules<TDefinedFor> Ignore<TPropertyType>(Expression<Func<TDefinedFor, TPropertyType>> property)
		{
			this.aIgnoreProperties.Add(Class<TDefinedFor>.GetProperty(property));

			return this;
		}

		public ClassDiffRules<TDefinedFor> AlwaysInclude<TPropertyType>(Expression<Func<TDefinedFor, TPropertyType>> property)
		{
			this.aAlwaysIncludedProperties.Add(Class<TDefinedFor>.GetProperty(property));

			return this;
		}

		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			this.ValidateType<TType>();

			return new ClassDiff<TType>(this.MergerImplementation, ((IClassDiffRules)this).IgnoredProperties, ((IClassDiffRules)this).AlwaysIncludedProperties);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffClassItem);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		bool IInheritableAlgorithmRules.IsInheritable { get; set; }

		IAlgorithmRules IInheritableAlgorithmRules.InheritedFrom { get; set; }

		#endregion

		#region Implementation of IClassDiffRules

		IEnumerable<Property> IClassDiffRules.IgnoredProperties
		{
			get
			{
				IEnumerable<Property> ignored = this.aIgnoreProperties;

				if (((IInheritableAlgorithmRules) this).InheritedFrom is IClassDiffRules baseRule)
					ignored = ignored.Union(baseRule.IgnoredProperties);

				return ignored;
			}
		}

		IEnumerable<Property> IClassDiffRules.AlwaysIncludedProperties
		{
			get
			{
				IEnumerable<Property> included = this.aAlwaysIncludedProperties;

				if (((IInheritableAlgorithmRules) this).InheritedFrom is IClassDiffRules baseRule)
					included = included.Union(baseRule.AlwaysIncludedProperties);

				return included;
			}
		}

		#endregion
	}
}
