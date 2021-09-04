using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using UnityEngine;

namespace Signals
{
    /// <summary><para>Implements the <see cref="T:System.Collections.Generic.IList&lt;T&gt;" /> interface. The size of a List is dynamically increased as required. A List is not guaranteed to be sorted. It is the programmer's responsibility to sort the List prior to performing operations (such as <see langword="BinarySearch" />) that require a List to be sorted. Indexing operations are required to perform in constant access time; that is, O(1).</para></summary>
    /// <typeparam name="T">To be added.</typeparam>
    /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.List%601">`List` on docs.go-mono.com</a></footer>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class SignalList<T> :
        ICollection<T>,
        IEnumerable<T>,
        IEnumerable,
        IList<T>,
        IReadOnlyCollection<T>,
        IReadOnlyList<T>,
        ICollection,
        IList
    {
        private List<T> _value = default;
        
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<T> value
        {
            get => _value;
            private set
            {
                if (_value != value)
                {
                    _value = value;
                    changed?.Invoke();
                }
            }
        }

        public Action changed { get; set; } = default;

        /// <summary>
        /// Triggers the <see cref="changed"/> event with the current value.
        /// </summary>
        public void ForceChange()
        {
            changed?.Invoke();
        }

        /// <summary><para>Initializes a new list that is empty and has the default initial capacity.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.List%601">`List` on docs.go-mono.com</a></footer>
        public SignalList()
        {
            _value = new List<T>();
        }

        /// <summary><para>Initializes a new list that is empty and has the default initial capacity.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.List%601">`List` on docs.go-mono.com</a></footer>
        public SignalList(IEnumerable<T> collection)
        {
            _value = new List<T>(collection);
        }

        /// <summary><para>Initializes a new list that is empty and has the default initial capacity.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.List%601">`List` on docs.go-mono.com</a></footer>
        public SignalList(int capacity)
        {
            _value = new List<T>(capacity);
        }

        /// <summary><para>Gets or sets the number of elements the current instance can contain.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Capacity">`List.Capacity` on docs.go-mono.com</a></footer>
        public int Capacity
        {
            get => _value.Capacity;
            set
            {
                if (_value.Capacity != value)
                {
                    _value.Capacity = value;
                    changed?.Invoke();
                }
            }
        }

        /// <summary><para>Gets the number of elements contained in the current instance.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Count">`List.Count` on docs.go-mono.com</a></footer>
        public int Count
        {
            get => _value.Count;
        }

        /// <summary><para>Gets or sets the element at the specified index of the current instance.</para></summary>
        /// <param name="index">The zero-based index of the element in the current instance to get or set.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Item">`List.Item` on docs.go-mono.com</a></footer>
        public T this[int index]
        {
            get => _value[index];
            set
            {
                if (!Compare(_value[index], value))
                {
                    _value[index] = value;
                    changed?.Invoke();
                }
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get => ((ICollection<T>)_value).IsReadOnly;
        }

        bool ICollection.IsSynchronized
        {
            get => ((ICollection)_value).IsSynchronized;
        }

        object ICollection.SyncRoot
        {
            get => ((ICollection)_value).SyncRoot;
        }

        bool IList.IsFixedSize
        {
            get => ((IList)_value).IsFixedSize;
        }

        bool IList.IsReadOnly
        {
            get => ((IList)_value).IsReadOnly;
        }

        /// <summary><para>Gets or sets the element at the specified index of the current instance.</para></summary>
        /// <param name="index">The zero-based index of the element in the current instance to get or set.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.List%3CT%3E.Item">`List.Item` on docs.go-mono.com</a></footer>
        object IList.this[int index]
        {
            get => ((IList)_value)[index];
            set
            {
                if (((IList)_value)[index] != value)
                {
                    ((IList)_value)[index] = value;
                    changed?.Invoke();
                }
            }
        }

        /// <summary><para>Adds an item to the end of the list.</para></summary>
        /// <param name="item">The item to add to the end of the list. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Add">`List.Add` on docs.go-mono.com</a></footer>
        public void Add(T item)
        {
            _value.Add(item);
            changed?.Invoke();
        }

        /// <summary><para>Adds the elements of the specified collection to the end of the list.</para></summary>
        /// <param name="collection">The collection whose elements are added to the end of the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.AddRange">`List.AddRange` on docs.go-mono.com</a></footer>
        public void AddRange(IEnumerable<T> collection)
        {
            _value.AddRange(collection);
            changed?.Invoke();
        }

        /// <summary><para>Returns a read-only wrapper to the current List.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.AsReadOnly">`List.AsReadOnly` on docs.go-mono.com</a></footer>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return _value.AsReadOnly();
        }

        /// <summary><para>Searches a range of elements in the sorted list for an element using the specified comparer and returns the zero-based index of the element.</para></summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.BinarySearch">`List.BinarySearch` on docs.go-mono.com</a></footer>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return _value.BinarySearch(index, count, item, comparer);
        }

        /// <summary><para>Searches the entire sorted list for an element using the default comparer, and returns the zero-based index of the element.</para></summary>
        /// <param name="item">The element for which to search. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.BinarySearch">`List.BinarySearch` on docs.go-mono.com</a></footer>
        public int BinarySearch(T item)
        {
            return _value.BinarySearch(item);
        }

        /// <summary><para>Searches the entire sorted list for an element using the specified comparer and returns the zero-based index of the element.</para></summary>
        /// <param name="item">The element for which to search. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.BinarySearch">`List.BinarySearch` on docs.go-mono.com</a></footer>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return _value.BinarySearch(item, comparer);
        }

        /// <summary><para>Removes all elements from the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Clear">`List.Clear` on docs.go-mono.com</a></footer>
        public void Clear()
        {
            _value.Clear();
            changed?.Invoke();
        }

        /// <summary><para>Determines whether the list contains a specific value.</para></summary>
        /// <param name="item">The object to locate in the current collection. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Contains">`List.Contains` on docs.go-mono.com</a></footer>
        public bool Contains(T item)
        {
            return _value.Contains(item);
        }

        /// <summary>To be added.</summary>
        /// <typeparam name="TOutput">To be added.</typeparam>
        /// <param name="converter">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.ConvertAll">`List.ConvertAll` on docs.go-mono.com</a></footer>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return _value.ConvertAll(converter);
        }

        /// <summary><para>Copies a range of elements of the list to an array, starting at a particular index in the target array.</para></summary>
        /// <param name="index">The zero-based index in the source list at which copying begins.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            _value.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary><para>Copies the entire list to an array.</para></summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements copied from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        public void CopyTo(T[] array)
        {
            _value.CopyTo(array);
        }

        /// <summary><para>Copies the elements of the list to an array, starting at a particular index.</para></summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements copied from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _value.CopyTo(array, arrayIndex);
        }

        /// <summary><para>Determines whether the List contains elements that match the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Exists">`List.Exists` on docs.go-mono.com</a></footer>
        public bool Exists(Predicate<T> match)
        {
            return _value.Exists(match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Find">`List.Find` on docs.go-mono.com</a></footer>
        public T Find(Predicate<T> match)
        {
            return _value.Find(match);
        }

        /// <summary><para>Retrieves all the elements that match the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the elements to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindAll">`List.FindAll` on docs.go-mono.com</a></footer>
        public List<T> FindAll(Predicate<T> match)
        {
            return _value.FindAll(match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the List that starts at the specified index and contains the specified number of elements.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindIndex">`List.FindIndex` on docs.go-mono.com</a></footer>
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            return _value.FindIndex(startIndex, count, match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the List that extends from the specified index to the last element.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindIndex">`List.FindIndex` on docs.go-mono.com</a></footer>
        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return _value.FindIndex(startIndex, match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindIndex">`List.FindIndex` on docs.go-mono.com</a></footer>
        public int FindIndex(Predicate<T> match)
        {
            return _value.FindIndex(match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLast">`List.FindLast` on docs.go-mono.com</a></footer>
        public T FindLast(Predicate<T> match)
        {
            return _value.FindLast(match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the List that starts at the specified index and contains the specified number of elements going backwards.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLastIndex">`List.FindLastIndex` on docs.go-mono.com</a></footer>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return _value.FindLastIndex(startIndex, count, match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the List that extends from the specified index to the first element.</para></summary>
        /// <param name="startIndex">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLastIndex">`List.FindLastIndex` on docs.go-mono.com</a></footer>
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return _value.FindLastIndex(startIndex, match);
        }

        /// <summary><para>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the List.</para></summary>
        /// <param name="match">The predicate delegate that specifies the element to search for.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.FindLastIndex">`List.FindLastIndex` on docs.go-mono.com</a></footer>
        public int FindLastIndex(Predicate<T> match)
        {
            return _value.FindLastIndex(match);
        }

        /// <summary><para>Performs the specified action on each element of the List.</para></summary>
        /// <param name="action">The action delegate to perform on each element of the List.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.ForEach">`List.ForEach` on docs.go-mono.com</a></footer>
        public void ForEach(Action<T> action)
        {
            _value.ForEach(action);
        }

        /// <summary><para>Returns an enumerator, in index order, that can be used to iterate over the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetEnumerator">`List.GetEnumerator` on docs.go-mono.com</a></footer>
        public List<T>.Enumerator GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        /// <summary><para>Creates a shallow copy of a range of elements in the current List.</para></summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetRange">`List.GetRange` on docs.go-mono.com</a></footer>
        public List<T> GetRange(int index, int count)
        {
            return _value.GetRange(index, count);
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the entire list.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        public int IndexOf(T item)
        {
            return _value.IndexOf(item);
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the list that extends from the specified index to the last element.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        public int IndexOf(T item, int index)
        {
            return _value.IndexOf(item, index);
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the list that starts at the specified index and contains the specified number of elements.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        public int IndexOf(T item, int index, int count)
        {
            return _value.IndexOf(item, index, count);
        }

        /// <summary><para>Inserts an item to the List at the specified position.</para></summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> is to be inserted.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Insert">`List.Insert` on docs.go-mono.com</a></footer>
        public void Insert(int index, T item)
        {
            _value.Insert(index, item);
            changed?.Invoke();
        }

        /// <summary><para>Inserts the elements of a collection in the List at the specified position.</para></summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.InsertRange">`List.InsertRange` on docs.go-mono.com</a></footer>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _value.InsertRange(index, collection);
            changed?.Invoke();
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the last occurrence within the entire list.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.LastIndexOf">`List.LastIndexOf` on docs.go-mono.com</a></footer>
        public int LastIndexOf(T item)
        {
            return _value.LastIndexOf(item);
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the list that extends from the specified index to the last element.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.LastIndexOf">`List.LastIndexOf` on docs.go-mono.com</a></footer>
        public int LastIndexOf(T item, int index)
        {
            return _value.LastIndexOf(item, index);
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the list that starts at the specified index and contains the specified number of elements.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.LastIndexOf">`List.LastIndexOf` on docs.go-mono.com</a></footer>
        public int LastIndexOf(T item, int index, int count)
        {
            return _value.LastIndexOf(item, index, count);
        }

        /// <summary><para>Removes the first occurrence of the specified object from the list.</para></summary>
        /// <param name="item">The object to be removed from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Remove">`List.Remove` on docs.go-mono.com</a></footer>
        public bool Remove(T item)
        {
            bool output = _value.Remove(item);
            if (output) changed?.Invoke();
            return output;
        }

        /// <summary><para>Removes the all the elements that match the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the elements to remove.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.RemoveAll">`List.RemoveAll` on docs.go-mono.com</a></footer>
        public int RemoveAll(Predicate<T> match)
        {
            int matches = _value.RemoveAll(match);
            if (matches > 0) changed?.Invoke();
            return matches;
        }

        /// <summary><para>Removes the item at the specified index of the list.</para></summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.RemoveAt">`List.RemoveAt` on docs.go-mono.com</a></footer>
        public void RemoveAt(int index)
        {
            _value.RemoveAt(index);
            changed?.Invoke();
        }

        /// <summary><para>Removes a range of elements from the list.</para></summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.RemoveRange">`List.RemoveRange` on docs.go-mono.com</a></footer>
        public void RemoveRange(int index, int count)
        {
            _value.RemoveRange(index, count);
            changed?.Invoke();
        }

        /// <summary><para>Reverses the order of the elements in the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Reverse">`List.Reverse` on docs.go-mono.com</a></footer>
        public void Reverse()
        {
            _value.Reverse();
            changed?.Invoke();
        }

        /// <summary><para>Reverses the order of the elements in the specified element range of the list.</para></summary>
        /// <param name="index">The zero-based starting index of the range of elements to reverse.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Reverse">`List.Reverse` on docs.go-mono.com</a></footer>
        public void Reverse(int index, int count)
        {
            _value.Reverse(index, count);
            changed?.Invoke();
        }

        /// <summary><para>Sorts the elements in the list using the default comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort()
        {
            _value.Sort();
            changed?.Invoke();
        }

        /// <summary><para>Sorts the elements in the list using the specified comparer.</para></summary>
        /// <param name="comparer"><para>The <see cref="T:System.Collections.Generic.IComparer&lt;T&gt;" />  implementation to use when comparing elements.</para><para>-or-</para><para><see langword="null" /> to use the default comparer.</para></param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort(IComparer<T> comparer)
        {
            _value.Sort(comparer);
            changed?.Invoke();
        }

        /// <summary><para>Sorts the elements in the list using the specified comparison.</para></summary>
        /// <param name="comparison"><para>The comparison to use when comparing elements.</para></param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort(Comparison<T> comparison)
        {
            _value.Sort(comparison);
            changed?.Invoke();
        }

        /// <summary><para>Sorts the elements in the list using the specified comparer.</para></summary>
        /// <param name="index">The zero-based starting index of the range of elements to sort.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Sort">`List.Sort` on docs.go-mono.com</a></footer>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            _value.Sort(index, count, comparer);
            changed?.Invoke();
        }

        /// <summary><para>Returns an enumerator, in index order, that can be used to iterate over the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetEnumerator">`List.GetEnumerator` on docs.go-mono.com</a></footer>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        /// <summary><para>Copies the elements of the list to an array, starting at a particular index.</para></summary>
        /// <param name="array">A one-dimensional, zero-based array that is the destination of the elements copied from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.CopyTo">`List.CopyTo` on docs.go-mono.com</a></footer>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            ((ICollection)_value).CopyTo(array, arrayIndex);
        }

        /// <summary><para>Returns an enumerator, in index order, that can be used to iterate over the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.GetEnumerator">`List.GetEnumerator` on docs.go-mono.com</a></footer>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        /// <summary><para>Adds an item to the end of the list.</para></summary>
        /// <param name="item">The item to add to the end of the list. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Add">`List.Add` on docs.go-mono.com</a></footer>
        int IList.Add(object item)
        {
            int output = ((IList)_value).Add(item);
            changed?.Invoke();
            return output;
        }

        /// <summary><para>Determines whether the list contains a specific value.</para></summary>
        /// <param name="item">The object to locate in the current collection. (<paramref name="item" /> can be <see langword="null" /> if T is a reference type.)</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Contains">`List.Contains` on docs.go-mono.com</a></footer>
        bool IList.Contains(object item)
        {
            return ((IList)_value).Contains(item);
        }

        /// <summary><para>Searches for the specified object and returns the zero-based index of the first occurrence within the entire list.</para></summary>
        /// <param name="item">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.IndexOf">`List.IndexOf` on docs.go-mono.com</a></footer>
        int IList.IndexOf(object item)
        {
            return ((IList)_value).IndexOf(item);
        }

        /// <summary><para>Inserts an item to the List at the specified position.</para></summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> is to be inserted.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Insert">`List.Insert` on docs.go-mono.com</a></footer>
        void IList.Insert(int index, object item)
        {
            ((IList)_value).Insert(index, item);
            changed?.Invoke();
        }

        /// <summary><para>Removes the first occurrence of the specified object from the list.</para></summary>
        /// <param name="item">The object to be removed from the list.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.Remove">`List.Remove` on docs.go-mono.com</a></footer>
        void IList.Remove(object item)
        {
            ((IList)_value).Remove(item);
            changed?.Invoke();
        }

        /// <summary><para>Copies the elements in the list to a new array.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.ToArray">`List.ToArray` on docs.go-mono.com</a></footer>
        public T[] ToArray()
        {
            return _value.ToArray();
        }

        /// <summary><para>Suggests that the capacity be reduced to the actual number of elements in the list.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.TrimExcess">`List.TrimExcess` on docs.go-mono.com</a></footer>
        public void TrimExcess()
        {
            _value.TrimExcess();
            changed?.Invoke();
        }

        /// <summary><para>Determines whether every element in the List matches the conditions defined by the specified predicate.</para></summary>
        /// <param name="match">The predicate delegate that specifies the check against the elements.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.List%3CT%3E.TrueForAll">`List.TrueForAll` on docs.go-mono.com</a></footer>
        public bool TrueForAll(Predicate<T> match)
        {
            return _value.TrueForAll(match);
        }

        static bool Compare(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }
    }
}