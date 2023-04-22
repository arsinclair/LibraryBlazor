using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataAccessLibrary
{
    /// <summary>
    ///   <para>Represents a strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists. Extends Collection.</para>
    /// </summary>
    /// <typeparam name="T">
    ///   <para>The type of elements in the list.</para>
    /// </typeparam>
    [Serializable]
    public class DataCollection<T> : Collection<T>
    {
        internal DataCollection()
        {
        }

        internal DataCollection(IList<T> list) => this.AddRange((IEnumerable<T>)list);

        /// <summary>
        ///   <para>Adds the elements of the specified collection to the end of the <see cref="T:DataAccessLibrary.DataCollection`1" />.</para>
        /// </summary>
        /// <param name="items">
        ///   <para>Type: T[]. The array whose elements should be added to the end of the <see cref="T:DataAccessLibrary.DataCollection`1" />. The array itself cannot be null, but it can contain elements that are null, if type <paramref name="T" /> is a reference type.</para>
        /// </param>
        public void AddRange(params T[] items)
        {
            if (items == null)
                return;
            this.AddRange((IEnumerable<T>)items);
        }

        /// <summary>
        ///   <para>Adds the elements of the specified collection to the end of the <see langword="DataCollection" />.</para>
        /// </summary>
        /// <param name="items">
        ///  <para>Type: IEnumerable&lt;T&gt;.
        /// The collection whose elements should be added to the end of the <see cref="T:DataAccessLibrary.DataCollection`1" />. The collection itself cannot be null, but it can contain elements that are null, if type <paramref name="T" /> is a reference type.</para>
        /// </param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                return;
            foreach (T obj in items)
                this.Add(obj);
        }

        /// <summary>
        ///   <para>Copies the elements of the <see cref="T:DataAccessLibrary.DataCollection`1" /> to a new array.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: Type[]
        /// An array containing copies of the elements of the <see cref="T:DataAccessLibrary.DataCollection`1" />.</para>
        /// </returns>
        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            this.CopyTo(array, 0);
            return array;
        }
    }
}
