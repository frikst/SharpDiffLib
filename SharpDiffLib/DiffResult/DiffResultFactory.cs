using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SharpDiffLib.diffResult.action;
using SharpDiffLib.diffResult.@base;
using SharpDiffLib.diffResult.implementation;
using SharpDiffLib.fastReflection;

namespace SharpDiffLib.diffResult
{
	public static class DiffResultFactory<TType>
	{
		public class Class
		{
			private readonly List<IDiffItem> aDiffItems;
		
			private Class()
			{
				this.aDiffItems = new List<IDiffItem>();
			}

			public static Class Create()
			{
				return new Class();
			}

			public Class Changed<TValue>(Expression<Func<TType, TValue>> property, IDiff<TValue> diff)
			{
				var p = Class<TType>.GetProperty(property);

				if (p == null)
					throw new Exception("Property does not exist in the selected class");

				this.aDiffItems.Add(new DiffClassChanged<TValue>(p, diff));
				return this;
			}

			public Class Replaced<TValue>(Expression<Func<TType, TValue>> property, TValue oldValue, TValue newValue)
			{
				var p = Class<TType>.GetProperty(property);

				if (p == null)
					throw new Exception("Property does not exist in the selected class");

				this.aDiffItems.Add(new DiffClassReplaced<TValue>(p, oldValue, newValue));
				return this;
			}

			public Class Conflicted(Action<Class> left, Action<Class> right)
			{
				Class conflictsLeft = new Class();
				left(conflictsLeft);

				Class conflictsRight = new Class();
				right(conflictsRight);

				this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

				return this;
			}

			public Class Unchanged<TValue>(Expression<Func<TType, TValue>> property, TValue value)
			{
				var p = Class<TType>.GetProperty(property);

				if (p == null)
					throw new Exception("Property does not exist in the selected class");

				this.aDiffItems.Add(new DiffClassUnchanged<TValue>(p, value));
				return this;
			}

			public Class Custom(IDiffItem item)
			{
				this.aDiffItems.Add(item);
				return this;
			}

			public IDiff<TType> MakeDiff(params IDiffItem[] diffItems)
			{
				return new Diff<TType>(this.aDiffItems);
			}
		}

		public class KeyValue<TKey, TValue>
		{
			private readonly List<IDiffItem> aDiffItems;
		
			private KeyValue()
			{
				this.aDiffItems = new List<IDiffItem>();
			}

			public static KeyValue<TKey, TValue> Create()
			{
				return new KeyValue<TKey, TValue>();
			}

			public KeyValue<TKey, TValue> Added(TKey key, TValue newValue)
			{
				this.aDiffItems.Add(new DiffKeyValueCollectionItemAdded<TKey, TValue>(key, newValue));
				return this;
			}

			public KeyValue<TKey, TValue> Removed(TKey key, TValue oldValue)
			{
				this.aDiffItems.Add(new DiffKeyValueCollectionItemRemoved<TKey, TValue>(key, oldValue));
				return this;
			}

			public KeyValue<TKey, TValue> Replaced(TKey key, TValue oldValue, TValue newValue)
			{
				this.aDiffItems.Add(new DiffKeyValueCollectionItemReplaced<TKey, TValue>(key, oldValue, newValue));
				return this;
			}

			public KeyValue<TKey, TValue> Changed(TKey key, IDiff<TValue> diff)
			{
				this.aDiffItems.Add(new DiffKeyValueCollectionItemChanged<TKey, TValue>(key, diff));
				return this;
			}

			public KeyValue<TKey, TValue> Conflicted(Action<KeyValue<TKey, TValue>> left, Action<KeyValue<TKey, TValue>> right)
			{
				KeyValue<TKey, TValue> conflictsLeft = new KeyValue<TKey, TValue>();
				left(conflictsLeft);

				KeyValue<TKey, TValue> conflictsRight = new KeyValue<TKey, TValue>();
				right(conflictsRight);

				this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

				return this;
			}

			public KeyValue<TKey, TValue> Unchanged(TKey key, TValue value)
			{
				this.aDiffItems.Add(new DiffKeyValueCollectionItemUnchanged<TKey, TValue>(key, value));
				return this;
			}

			public KeyValue<TKey, TValue> Custom(IDiffItem item)
			{
				this.aDiffItems.Add(item);
				return this;
			}

			public IDiff<TType> MakeDiff(params IDiffItem[] diffItems)
			{
				return new Diff<TType>(this.aDiffItems);
			}
		}

		public class Ordered<TValue>
		{
			private readonly List<IDiffItem> aDiffItems;
		
			private Ordered()
			{
				this.aDiffItems = new List<IDiffItem>();
			}

			public static Ordered<TValue> Create()
			{
				return new Ordered<TValue>();
			}

			public Ordered<TValue> Added(int index, TValue newValue)
			{
				this.aDiffItems.Add(new DiffOrderedCollectionAdded<TValue>(index, newValue));
				return this;
			}

			public Ordered<TValue> Removed(int index, TValue oldValue)
			{
				this.aDiffItems.Add(new DiffOrderedCollectionRemoved<TValue>(index, oldValue));
				return this;
			}

			public Ordered<TValue> Replaced(int index, TValue oldValue, TValue newValue)
			{
				this.aDiffItems.Add(new DiffOrderedCollectionReplaced<TValue>(index, oldValue, newValue));
				return this;
			}

			public Ordered<TValue> Changed(int index, IDiff<TValue> diff)
			{
				this.aDiffItems.Add(new DiffOrderedCollectionChanged<TValue>(index, diff));
				return this;
			}

			public Ordered<TValue> Conflicted(Action<Ordered<TValue>> left, Action<Ordered<TValue>> right)
			{
				Ordered<TValue> conflictsLeft = new Ordered<TValue>();
				left(conflictsLeft);

				Ordered<TValue> conflictsRight = new Ordered<TValue>();
				right(conflictsRight);

				this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

				return this;
			}

			public Ordered<TValue> Unchanged(int index, TValue value)
			{
				this.aDiffItems.Add(new DiffOrderedCollectionUnchanged<TValue>(index, value));
				return this;
			}

			public Ordered<TValue> Custom(IDiffItem item)
			{
				this.aDiffItems.Add(item);
				return this;
			}

			public IDiff<TType> MakeDiff(params IDiffItem[] diffItems)
			{
				return new Diff<TType>(this.aDiffItems);
			}
		}

		public class Unordered<TValue>
		{
			private readonly List<IDiffItem> aDiffItems;
		
			private Unordered()
			{
				this.aDiffItems = new List<IDiffItem>();
			}

			public static Unordered<TValue> Create()
			{
				return new Unordered<TValue>();
			}

			public Unordered<TValue> Added(TValue newValue)
			{
				this.aDiffItems.Add(new DiffUnorderedCollectionAdded<TValue>(newValue));
				return this;
			}

			public Unordered<TValue> Removed(TValue oldValue)
			{
				this.aDiffItems.Add(new DiffUnorderedCollectionRemoved<TValue>(oldValue));
				return this;
			}

			public Unordered<TValue> Replaced(TValue oldValue, TValue newValue)
			{
				this.aDiffItems.Add(new DiffUnorderedCollectionReplaced<TValue>(oldValue, newValue));
				return this;
			}

			public Unordered<TValue> Changed<TId>(TId id, IDiff<TValue> diff)
			{
				this.aDiffItems.Add(new DiffUnorderedCollectionChanged<TId, TValue>(id, diff));
				return this;
			}

			public Unordered<TValue> Conflicted(Action<Unordered<TValue>> left, Action<Unordered<TValue>> right)
			{
				Unordered<TValue> conflictsLeft = new Unordered<TValue>();
				left(conflictsLeft);

				Unordered<TValue> conflictsRight = new Unordered<TValue>();
				right(conflictsRight);

				this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

				return this;
			}

			public Unordered<TValue> Unchanged(TValue value)
			{
				this.aDiffItems.Add(new DiffUnorderedCollectionUnchanged<TValue>(value));
				return this;
			}

			public Unordered<TValue> Custom(IDiffItem item)
			{
				this.aDiffItems.Add(item);
				return this;
			}

			public IDiff<TType> MakeDiff(params IDiffItem[] diffItems)
			{
				return new Diff<TType>(this.aDiffItems);
			}
		}

		public class Value
		{
			private readonly List<IDiffItem> aDiffItems;

			private Value()
			{
				this.aDiffItems = new List<IDiffItem>();
			}

			public static Value Create()
			{
				return new Value();
			}

			public Value Replaced(TType oldValue, TType newValue)
			{
				this.aDiffItems.Add(new DiffValueReplaced<TType>(oldValue, newValue));
				return this;
			}

			public Value Changed<TSubType>(IDiff<TSubType> diff)
				where TSubType : TType
			{
				this.aDiffItems.Add(new DiffValueChanged<TType>(typeof(TSubType), (IDiff<TType>) diff));
				return this;
			}

			public Value Changed(IDiff<TType> diff)
			{
				this.aDiffItems.Add(new DiffValueChanged<TType>(typeof(TType), diff));
				return this;
			}

			public Value Conflicted(Action<Value> left, Action<Value> right)
			{
				Value conflictsLeft = new Value();
				left(conflictsLeft);

				Value conflictsRight = new Value();
				right(conflictsRight);

				this.aDiffItems.Add(new DiffAnyConflicted(conflictsLeft.aDiffItems, conflictsRight.aDiffItems));

				return this;
			}

			public Value Unchanged(TType value)
			{
				this.aDiffItems.Add(new DiffValueUnchanged<TType>(value));
				return this;
			}

			public Value Custom(IDiffItem item)
			{
				this.aDiffItems.Add(item);
				return this;
			}

			public IDiff<TType> MakeDiff(params IDiffItem[] diffItems)
			{
				return new Diff<TType>(this.aDiffItems);
			}
		}
	}

	public static class DiffResultFactory
	{
		public static class Class<TType>
		{
			public static DiffResultFactory<TType>.Class Create()
			{
				return DiffResultFactory<TType>.Class.Create();
			}
		}

		public static class KeyValue<TKey, TValue>
		{
			public static DiffResultFactory<Dictionary<TKey, TValue>>.KeyValue<TKey, TValue> Create()
			{
				return DiffResultFactory<Dictionary<TKey, TValue>>.KeyValue<TKey, TValue>.Create();
			}
		}

		public static class Ordered<TValue>
		{
			public static DiffResultFactory<TValue[]>.Ordered<TValue> Create()
			{
				return DiffResultFactory<TValue[]>.Ordered<TValue>.Create();
			}
		}

		public static class Unordered<TValue>
		{
			public static DiffResultFactory<HashSet<TValue>>.Unordered<TValue> Create()
			{
				return DiffResultFactory<HashSet<TValue>>.Unordered<TValue>.Create();
			}
		}

		public static class Value<TType>
		{
			public static DiffResultFactory<TType>.Value Create()
			{
				return DiffResultFactory<TType>.Value.Create();
			}
		}
	}
}
