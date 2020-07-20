using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    [Serializable]
    public abstract class DataCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private IDictionary<TKey, TValue> _innerDictionary = (IDictionary<TKey, TValue>)new Dictionary<TKey, TValue>();
        private bool _isReadOnly;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.DataCollection" /> class.</para>
        /// </summary>
        /// <remarks />
        protected internal DataCollection()
        {
        }


        /// <summary>
        ///     <para>Adds the specified key and value to the dictionary.</para>
        /// </summary>
        /// <param name="item">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/5tbh8a42.aspx">KeyValuePair</see>&lt;TKey, TValue&gt;. The key and value to add.</para>
        /// </param>
        /// <remarks />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.CheckIsReadOnly();
            this._innerDictionary.Add(item);
        }

        /// <summary>
        ///     <para>Adds the elements of the specified collection to the end of the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <param name="items">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/5tbh8a42.aspx">KeyValuePair</see>&lt;TKey, TValue&gt;[].
        /// The array whose elements should be added to the end of the <see cref="T:DataAccessLibrary.DataCollection" />. The array itself cannot be null, but it can contain elements that are null, if type <paramref name="T" /> is a reference type.</para>
        /// </param>
        /// <remarks />
        public void AddRange(params KeyValuePair<TKey, TValue>[] items)
        {
            this.CheckIsReadOnly();
            this.AddRange((IEnumerable<KeyValuePair<TKey, TValue>>)items);
        }

        /// <summary>
        ///     <para>Adds the elements of the specified collection to the end of the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <param name="items">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.collections.ienumerable.aspx">IEnumerable</see>&lt;<see href="https://msdn.microsoft.com/library/5tbh8a42.aspx">KeyValuePair</see>&lt;TKey, TValue&gt;&gt;.
        /// The collection whose elements should be added to the end of the <see cref="T:DataAccessLibrary.DataCollection" />. The collection itself cannot be null, but it can contain elements that are null, if type <paramref name="T" /> is a reference type.</para>
        /// </param>
        /// <remarks />
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
            {
                return;
            }
            this.CheckIsReadOnly();
            ICollection<KeyValuePair<TKey, TValue>> innerDictionary = (ICollection<KeyValuePair<TKey, TValue>>)this._innerDictionary;
            foreach (KeyValuePair<TKey, TValue> keyValuePair in items)
            {
                innerDictionary.Add(keyValuePair);
            }
        }

        /// <summary>
        ///     <para>Adds the specified key and value to the dictionary.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: TKey. The key of the element to add.</para>
        /// </param>
        /// <param name="value">
        ///     <para>Type: TValue. The value of the element to add.</para>
        /// </param>
        /// <remarks />
        public void Add(TKey key, TValue value)
        {
            this.CheckIsReadOnly();
            this._innerDictionary.Add(key, value);
        }

        /// <summary>
        ///     <para>Gets or sets the value associated with the specified key.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: TKey. The key of the value to get or set.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see langword="TValue" />
        /// The value associated with the specified key.</para>
        /// </returns>
        /// <remarks />
        public virtual TValue this[TKey key]
        {
            get
            {
                return this._innerDictionary[key];
            }
            set
            {
                this.CheckIsReadOnly();
                this._innerDictionary[key] = value;
            }
        }

        /// <summary>
        ///   <para>Removes all items from the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <remarks />
        public void Clear()
        {
            this.CheckIsReadOnly();
            this._innerDictionary.Clear();
        }

        /// <summary>
        ///     <para>Determines whether the <see cref="T:DataAccessLibrary.DataCollection" /> contains a specific value.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: TKey. The key to locate in the collection.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if item is found in the <see cref="T:DataAccessLibrary.DataCollection" />; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool Contains(TKey key)
        {
            return this._innerDictionary.ContainsKey(key);
        }

        /// <summary>
        ///     <para>Determines whether the <see cref="T:DataAccessLibrary.DataCollection" /> contains a specific value.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/5tbh8a42.aspx">KeyValuePair</see>&lt;TKey, TValue&gt;. The key value pair to locate in the collection.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if item is found in the <see cref="T:DataAccessLibrary.DataCollection" />; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool Contains(KeyValuePair<TKey, TValue> key)
        {
            return this._innerDictionary.Contains(key);
        }

        /// <summary>
        ///     <para>Gets the value associated with the specified key.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: TKey. The key of the value to get.</para>
        /// </param>
        /// <param name="value">
        ///     <para>Type: TValue%. When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the <see cref="T:DataAccessLibrary.DataCollection" /> contains an element with the specified key; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._innerDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        ///     <para />
        /// </summary>
        /// <param name="array">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/5tbh8a42.aspx">KeyValuePair</see>&lt;TKey, TValue&gt;[]. The </para>
        /// </param>
        /// <param name="arrayIndex">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.int32.aspx">Int32</see>. Copies the entire <see cref="T:DataAccessLibrary.DataCollection" /> to a compatible one-dimensional array, starting at the specified index of the target array.</para>
        /// </param>
        /// <remarks />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this._innerDictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     <para>Determines whether the <see cref="T:DataAccessLibrary.DataCollection" /> contains a specific key value.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: TKey. The key to locate in the collection.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if item is found in the <see cref="T:DataAccessLibrary.DataCollection" />; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool ContainsKey(TKey key)
        {
            return this._innerDictionary.ContainsKey(key);
        }

        /// <summary>
        ///     <para>Removes the first occurrence of a specific object from the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <param name="key">
        ///     <para>Type: TKey. The object to remove from the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the object was found and removed; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool Remove(TKey key)
        {
            this.CheckIsReadOnly();
            return this._innerDictionary.Remove(key);
        }

        /// <summary>
        ///     <para>Removes the first occurrence of a specific object from the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <param name="item">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/5tbh8a42.aspx">KeyValuePair</see>&lt;TKey, TValue&gt;.
        /// The object to remove from the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the object was found and removed; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            this.CheckIsReadOnly();
            return ((ICollection<KeyValuePair<TKey, TValue>>)this._innerDictionary).Remove(item);
        }

        /// <summary>
        ///     <para>Gets the number of elements in the collection.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.int32.aspx">Int32</see>
        /// The number of elements in the collection.</para>
        /// </returns>
        /// <remarks />
        public int Count
        {
            get
            {
                return this._innerDictionary.Count;
            }
        }

        /// <summary>
        ///     <para>Gets a collection containing the keys in the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/92t2ye13">ICollection</see>&lt;TKey&gt;
        /// A collection containing the keys in the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </returns>
        /// <remarks />
        public ICollection<TKey> Keys
        {
            get
            {
                return this._innerDictionary.Keys;
            }
        }

        /// <summary>
        ///     <para>Gets a collection containing the values in the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/92t2ye13">ICollection</see>&lt;TValue&gt;
        /// A collection containing the values in the <see cref="T:DataAccessLibrary.DataCollection" />.</para>
        /// </returns>
        /// <remarks />
        public ICollection<TValue> Values
        {
            get
            {
                return this._innerDictionary.Values;
            }
        }

        /// <summary>
        ///     <para>Gets a value indicating whether the <see cref="T:DataAccessLibrary.DataCollection" /> is read-only.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the <see cref="T:DataAccessLibrary.DataCollection" /> is read-only; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public virtual bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
            internal set
            {
                this._isReadOnly = value;
            }
        }

        internal void SetItemInternal(TKey key, TValue value)
        {
            this._innerDictionary[key] = value;
        }

        internal void ClearInternal()
        {
            this._innerDictionary.Clear();
        }

        internal bool RemoveInternal(TKey key)
        {
            return this._innerDictionary.Remove(key);
        }

        private void CheckIsReadOnly()
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("The collection is read-only.");
        }

        /// <summary>
        ///     <para>Returns an enumerator that iterates through a collection.</para>
        /// </summary>
        /// <remarks />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._innerDictionary.GetEnumerator();
        }
    }
}