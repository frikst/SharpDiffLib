﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using POCOMerger.diffResult.@base;
using POCOMerger.diffResult.implementation;
using POCOMerger.fastReflection;

namespace POCOMerger.diffResult
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
				var p = Class<TType>.GetProperty(((MemberExpression)property.Body).Member);

				if (p == null)
					throw new Exception("Property does not exist in the selected class");

				this.aDiffItems.Add(new DiffClassChanged<TValue>(p, diff));
				return this;
			}

			public Class Replaced<TValue>(Expression<Func<TType, TValue>> property, TValue oldValue, TValue newValue)
			{
				var p = Class<TType>.GetProperty(((MemberExpression)property.Body).Member);

				if (p == null)
					throw new Exception("Property does not exist in the selected class");

				this.aDiffItems.Add(new DiffClassReplaced<TValue>(p, oldValue, newValue));
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
}