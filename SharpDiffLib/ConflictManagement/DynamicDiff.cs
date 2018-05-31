﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.implementation;

namespace SharpDiffLib.conflictManagement
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
			return new DynamicDiffIterator(this.aOriginal, this.aDynamicDiffItemChangedFactory, this.aResolveActions).GetEnumerator();
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

		#endregion

		#region Implementation of IDiff

		public string ToString(int indentLevel)
		{
			StringBuilder ret = new StringBuilder();

			foreach (IDiffItem diffItem in this)
				ret.AppendLine(diffItem.ToString(indentLevel));

			return ret.ToString().TrimEnd('\r', '\n');
		}

		public override string ToString()
		{
			return this.ToString(0);
		}

		public bool HasChanges
		{
			get { return this.aOriginal.HasChanges; }
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
			return new Diff<TType>(new DynamicDiffIterator(this.aOriginal, this.aFinishDiffItemChangedFactory, this.aResolveActions).ToList());
		}
	}
}