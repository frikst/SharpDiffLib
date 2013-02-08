using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using POCOMerger.diffResult.action;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.@internal;

namespace POCOMerger.conflictManagement
{
	internal class DynamicDiff<TType> : IDiff<TType>
	{
		private readonly IDiff<TType> aOriginal;
		private readonly Dictionary<IDiffItemConflicted, ResolveAction> aResolveActions;
		private readonly DynamicDiffItemChangedFactory aDynamicDiffItemChangedFactory;
		private readonly DynamicDiffItemChangedFactory aFinishDiffItemChangedFactory;

		public DynamicDiff(IDiff<TType> original, Dictionary<IDiffItemConflicted, ResolveAction> resolveActions)
		{
			this.aOriginal = original;
			this.aResolveActions = resolveActions;

			this.aDynamicDiffItemChangedFactory = new DynamicDiffItemChangedFactory(this.aResolveActions, false);
			this.aFinishDiffItemChangedFactory = new DynamicDiffItemChangedFactory(this.aResolveActions, true);
		}

		#region Implementation of IEnumerable<IDiffItem>

		public IEnumerator<IDiffItem> GetEnumerator()
		{
			return this.GetItems(this.aDynamicDiffItemChangedFactory).GetEnumerator();
		}

		#endregion

		#region Implementation of IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Implementation of ICountableEnumerable<IDiffItem>

		public int Count
		{
			get
			{
				int ret = 0;
				foreach (IDiffItem item in this.aOriginal)
				{
					if (item is IDiffItemConflicted)
					{
						IDiffItemConflicted conflict = (IDiffItemConflicted) item;

						ResolveAction action;
						if (this.aResolveActions.TryGetValue(conflict, out action))
						{
							switch (action)
							{
								case ResolveAction.UseLeft:
									ret += conflict.Left.Count;
									break;
								case ResolveAction.UseRight:
									ret += conflict.Right.Count;
									break;
								case ResolveAction.LeftThenRight:
								case ResolveAction.RightThenLeft:
									ret += conflict.Left.Count + conflict.Right.Count;
									break;
							}
						}
						else
							ret++;
					}
					else
						ret++;
				}

				return ret;
			}
		}

		public DynamicDiffItemChangedFactory DynamicDiffItemChangedFactory
		{
			get { return this.aDynamicDiffItemChangedFactory; }
		}

		#endregion

		#region Implementation of IDiff

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			foreach (IDiffItem diffItem in this)
				ret.AppendLine(diffItem.ToString(indentLevel));

			return ret.ToString().TrimEnd('\r', '\n');
		}

		#endregion

		#region Equality members

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (!(obj is IDiff<TType>))
				return false;

			return this.SequenceEqual((IDiff<TType>) obj);
		}

		public override int GetHashCode()
		{
			throw new Exception("Cannot compute hash code for diff");
		}

		#endregion

		public IDiff<TType> Finish()
		{
			return new Diff<TType>(this.GetItems(this.aDynamicDiffItemChangedFactory).ToList());
		}

		private IEnumerable<IDiffItem> GetItems(DynamicDiffItemChangedFactory dynamicDiffItemChangedFactory)
		{
			foreach (IDiffItem item in this.aOriginal)
			{
				if (item is IDiffItemConflicted)
				{
					IDiffItemConflicted conflict = (IDiffItemConflicted)item;

					ResolveAction action;
					if (this.aResolveActions.TryGetValue(conflict, out action))
					{
						switch (action)
						{
							case ResolveAction.UseLeft:
								foreach (var left in conflict.Left)
									yield return left;
								break;
							case ResolveAction.UseRight:
								foreach (var right in conflict.Right)
									yield return right;
								break;
							case ResolveAction.LeftThenRight:
								foreach (var left in conflict.Left)
									yield return left;
								foreach (var right in conflict.Right)
									yield return right;
								break;
							case ResolveAction.RightThenLeft:
								foreach (var right in conflict.Right)
									yield return right;
								foreach (var left in conflict.Left)
									yield return left;
								break;
						}
					}
					else
						yield return item;
				}
				else if (item is IDiffItemChanged)
					yield return dynamicDiffItemChangedFactory.Create((IDiffItemChanged)item);
				else
					yield return item;
			}
		}
	}
}