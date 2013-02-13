using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.algorithms.diff.@base;
using POCOMerger.definition.rules;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;

namespace POCOMerger.algorithms.diff.common.@class
{
	internal interface IClassDiffRules
	{
		IEnumerable<Property> IgnoredProperties { get; }
		IEnumerable<Property> AlwaysIncludedProperties { get; }
	}

	public class ClassDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>, IClassDiffRules
	{
		private readonly HashSet<Property> aIgnoreProperties;
		private readonly HashSet<Property> aAlwaysIncludedProperties;
		private IAlgorithmRules aInheritAfter;

		public ClassDiffRules()
		{
			this.aIgnoreProperties = new HashSet<Property>();
			this.aAlwaysIncludedProperties = new HashSet<Property>();
		}

		public ClassDiffRules<TDefinedFor> Ignore<TPropertyType>(Expression<Func<TDefinedFor, TPropertyType>> property)
		{
			this.aIgnoreProperties.Add(Class<TDefinedFor>.GetProperty(((MemberExpression)property.Body).Member));

			return this;
		}

		public ClassDiffRules<TDefinedFor> IncludeAlways<TPropertyType>(Expression<Func<TDefinedFor, TPropertyType>> property)
		{
			this.aAlwaysIncludedProperties.Add(Class<TDefinedFor>.GetProperty(((MemberExpression)property.Body).Member));

			return this;
		}

		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			return new ClassDiff<TType>(this.MergerImplementation, ((IClassDiffRules)this).IgnoredProperties, ((IClassDiffRules)this).AlwaysIncludedProperties);
		}

		IEnumerable<Type> IAlgorithmRules.GetPossibleResults()
		{
			yield return typeof(IDiffClassItem);
			yield return typeof(IDiffItemChanged);
			yield return typeof(IDiffItemReplaced);
		}

		IAlgorithmRules IAlgorithmRules.InheritAfter
		{
			get { return this.aInheritAfter; }
			set { this.aInheritAfter = value; }
		}

		#endregion

		#region Implementation of IClassDiffRules

		IEnumerable<Property> IClassDiffRules.IgnoredProperties
		{
			get
			{
				IEnumerable<Property> ignored = this.aIgnoreProperties;

				if (this.aInheritAfter is IClassDiffRules)
					ignored = ignored.Union(((IClassDiffRules) this.aInheritAfter).IgnoredProperties);

				return ignored;
			}
		}

		IEnumerable<Property> IClassDiffRules.AlwaysIncludedProperties
		{
			get
			{
				IEnumerable<Property> included = this.aAlwaysIncludedProperties;

				if (this.aInheritAfter is IClassDiffRules)
					included = included.Union(((IClassDiffRules)this.aInheritAfter).AlwaysIncludedProperties);

				return included;
			}
		}

		#endregion
	}
}
