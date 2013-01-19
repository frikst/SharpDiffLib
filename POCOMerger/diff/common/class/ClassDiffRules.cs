using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using POCOMerger.definition.rules;
using POCOMerger.diff.@base;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.type;
using POCOMerger.fastReflection;

namespace POCOMerger.diff.common.@class
{
	internal interface IClassDiffRules
	{
		IEnumerable<Property> IgnoredProperties { get; }
	}

	public class ClassDiffRules<TDefinedFor> : BaseRules<TDefinedFor>, IDiffAlgorithmRules<TDefinedFor>, IClassDiffRules
	{
		private HashSet<Property> aIgnoreProperties;
		private IAlgorithmRules aInheritAfter;

		public ClassDiffRules()
		{
			this.aIgnoreProperties = new HashSet<Property>();
		}

		public ClassDiffRules<TDefinedFor> Ignore<TPropertyType>(Expression<Func<TDefinedFor, TPropertyType>> property)
		{
			this.aIgnoreProperties.Add(Class<TDefinedFor>.GetProperty(((MemberExpression)property.Body).Member));

			return this;
		}

		#region Implementation of IDiffAlgorithmRules

		IDiffAlgorithm<TType> IDiffAlgorithmRules.GetAlgorithm<TType>()
		{
			IEnumerable<Property> ignored = this.aIgnoreProperties;

			if (this.aInheritAfter is IClassDiffRules)
				ignored = ignored.Union(((IClassDiffRules)this.aInheritAfter).IgnoredProperties);

			return new ClassDiff<TType>(this.MergerImplementation, ignored);
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
			get { return this.aIgnoreProperties; }
		}

		#endregion
	}
}
