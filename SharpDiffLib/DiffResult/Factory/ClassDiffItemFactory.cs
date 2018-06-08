using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KST.SharpDiffLib.DiffResult.Base;
using KST.SharpDiffLib.DiffResult.Implementation;
using KST.SharpDiffLib.FastReflection;

namespace KST.SharpDiffLib.DiffResult.Factory
{
	public class ClassDiffItemFactory<TType> : IDiffItemFactory<TType>
	{
		private readonly List<IDiffItem> aDiffItems;
	
		internal ClassDiffItemFactory()
		{
			this.aDiffItems = new List<IDiffItem>();
		}

		public ClassDiffItemFactory<TType> Changed<TValue>(Expression<Func<TType, TValue>> property, Func<InnerDiffFactory.IDiffFactory<TValue, TValue>, IDiffItemFactory<TValue>> diffFactory)
		{
			var diff = diffFactory(new InnerDiffFactory.DiffFactoryImplementation<TValue, TValue>()).MakeDiff();

			return this.Changed(property, diff);
		}

		public ClassDiffItemFactory<TType> Changed<TValue>(Expression<Func<TType, TValue>> property, IDiff<TValue> diff)
		{
			var p = Class<TType>.GetProperty(property);

			if (p == null)
				throw new Exception("Property does not exist in the selected class");

			this.aDiffItems.Add(new DiffClassChanged<TValue>(p, diff));
			return this;
		}

		public ClassDiffItemFactory<TType> Replaced<TValue>(Expression<Func<TType, TValue>> property, TValue oldValue, TValue newValue)
		{
			var p = Class<TType>.GetProperty(property);

			if (p == null)
				throw new Exception("Property does not exist in the selected class");

			this.aDiffItems.Add(new DiffClassReplaced<TValue>(p, oldValue, newValue));
			return this;
		}

		public ClassDiffItemFactory<TType> Conflicted(Action<ClassDiffItemFactory<TType>> left, Action<ClassDiffItemFactory<TType>> right)
		{
			var conflictsLeft = new ClassDiffItemFactory<TType>();
			left(conflictsLeft);

			var conflictsRight = new ClassDiffItemFactory<TType>();
			right(conflictsRight);

			return this.Conflicted(conflictsLeft, conflictsRight);
		}

		public ClassDiffItemFactory<TType> Conflicted(ClassDiffItemFactory<TType> conflictsLeft, ClassDiffItemFactory<TType> conflictsRight)
		{
			this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

			return this;
		}

		public ClassDiffItemFactory<TType> Unchanged<TValue>(Expression<Func<TType, TValue>> property, TValue value)
		{
			var p = Class<TType>.GetProperty(property);

			if (p == null)
				throw new Exception("Property does not exist in the selected class");

			this.aDiffItems.Add(new DiffClassUnchanged<TValue>(p, value));
			return this;
		}

		public ClassDiffItemFactory<TType> Custom(IDiffItem item)
		{
			this.aDiffItems.Add(item);
			return this;
		}

		public IDiff<TType> MakeDiff()
		{
			return new Diff<TType>(this.aDiffItems);
		}
	}
}
