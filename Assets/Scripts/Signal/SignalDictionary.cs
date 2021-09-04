using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using System.Linq;

namespace Signals
{
  /// <summary><para>Represents a collection of key/value pairs that are organized based on the key.</para></summary>
  /// <typeparam name="TKey">To be added.</typeparam>
  /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
    [Serializable]
    public class SignalDictionary<TKey, TValue> :
        ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable,
        IDictionary<TKey, TValue>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyDictionary<TKey, TValue>,
        ICollection,
        IDictionary,
        IDeserializationCallback,
        ISerializable
    {
        private Dictionary<TKey, TValue> _value = default;

        public Dictionary<TKey, TValue> value
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

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
        public SignalDictionary()
        {
          _value = new Dictionary<TKey, TValue>();
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
        public SignalDictionary(IDictionary<TKey, TValue> dictionary)
        {
          _value = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
        public SignalDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
          _value = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
        public SignalDictionary(IEqualityComparer<TKey> comparer)
        {
          _value = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
        public SignalDictionary(int capacity)
        {
          _value = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=T:System.Collections.Generic.Dictionary%602">`Dictionary` on docs.go-mono.com</a></footer>
        public SignalDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
          _value = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>To be added.</summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Comparer">`Dictionary.Comparer` on docs.go-mono.com</a></footer>
        public IEqualityComparer<TKey> Comparer { get => _value.Comparer; }

        /// <summary><para>Gets the number of key/value pairs contained in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Count">`Dictionary.Count` on docs.go-mono.com</a></footer>
        public int Count { get => _value.Count; }

        /// <summary><para>Gets or sets the value associated with the specified key.</para></summary>
        /// <param name="key">The key whose value is to be gotten or set.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Item">`Dictionary.Item` on docs.go-mono.com</a></footer>
        public TValue this[TKey key]
        {
          get => _value[key];
          set {
            _value[key] = value;
            changed?.Invoke();
          }
        }

        /// <summary><para>Gets a collection that contains the keys in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Keys">`Dictionary.Keys` on docs.go-mono.com</a></footer>
        public Dictionary<TKey, TValue>.KeyCollection Keys { get => _value.Keys; }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get => ((ICollection<KeyValuePair<TKey, TValue>>)_value).IsReadOnly; }

        /// <summary><para>Gets a collection that contains the keys in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Keys">`Dictionary.Keys` on docs.go-mono.com</a></footer>
        ICollection<TKey> IDictionary<TKey, TValue>.Keys { get => ((IDictionary<TKey, TValue>)_value).Keys; }

        /// <summary><para>Gets a collection that contains the values in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Values">`Dictionary.Values` on docs.go-mono.com</a></footer>
        ICollection<TValue> IDictionary<TKey, TValue>.Values { get => ((IDictionary<TKey, TValue>)_value).Values; }

        /// <summary><para>Gets a collection that contains the keys in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Keys">`Dictionary.Keys` on docs.go-mono.com</a></footer>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get => ((IReadOnlyDictionary<TKey, TValue>)_value).Keys; }

        /// <summary><para>Gets a collection that contains the values in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Values">`Dictionary.Values` on docs.go-mono.com</a></footer>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get => ((IReadOnlyDictionary<TKey, TValue>)_value).Values; }

        bool ICollection.IsSynchronized { get => ((ICollection)_value).IsSynchronized; }

        object ICollection.SyncRoot { get => ((ICollection)_value).SyncRoot; }

        bool IDictionary.IsFixedSize { get => ((IDictionary)_value).IsFixedSize; }

        bool IDictionary.IsReadOnly { get => ((IDictionary)_value).IsReadOnly; }

        /// <summary><para>Gets or sets the value associated with the specified key.</para></summary>
        /// <param name="key">The key whose value is to be gotten or set.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Item">`Dictionary.Item` on docs.go-mono.com</a></footer>
        object IDictionary.this[object key]
        {
          get => ((IDictionary)_value)[key];
          set
          {
            ((IDictionary)_value)[key] = value;
            changed?.Invoke();
          }
        }

        /// <summary><para>Gets a collection that contains the keys in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Keys">`Dictionary.Keys` on docs.go-mono.com</a></footer>
        ICollection IDictionary.Keys { get => ((IDictionary)_value).Keys; }

        /// <summary><para>Gets a collection that contains the values in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Values">`Dictionary.Values` on docs.go-mono.com</a></footer>
        ICollection IDictionary.Values { get => ((IDictionary)value).Values; }

        /// <summary><para>Gets a collection that contains the values in the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=P:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Values">`Dictionary.Values` on docs.go-mono.com</a></footer>
        public Dictionary<TKey, TValue>.ValueCollection Values { get => value.Values; }

        /// <summary><para>Adds an element with the specified key and value to the dictionary.</para></summary>
        /// <param name="key">The key of the element to add to the dictionary.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Add">`Dictionary.Add` on docs.go-mono.com</a></footer>
        public void Add(TKey key, TValue value)
        {
          _value.Add(key, value);
          changed?.Invoke();
        }

        /// <summary><para>Removes all elements from the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Clear">`Dictionary.Clear` on docs.go-mono.com</a></footer>
        public void Clear()
        {
          _value.Clear();
          changed?.Invoke();
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific key.</para></summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.ContainsKey">`Dictionary.ContainsKey` on docs.go-mono.com</a></footer>
        public bool ContainsKey(TKey key)
        {
          return _value.ContainsKey(key);
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific value.</para></summary>
        /// <param name="value">The value to locate in the dictionary.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.ContainsValue">`Dictionary.ContainsValue` on docs.go-mono.com</a></footer>
        public bool ContainsValue(TValue value)
        {
          return _value.ContainsValue(value);
        }

        /// <summary><para>Returns an enumerator that can be used to iterate over the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.GetEnumerator">`Dictionary.GetEnumerator` on docs.go-mono.com</a></footer>
        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
          return _value.GetEnumerator();
        }

        /// <summary>To be added.</summary>
        /// <param name="info">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.GetObjectData">`Dictionary.GetObjectData` on docs.go-mono.com</a></footer>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
          _value.GetObjectData(info, context);
        }

        /// <summary>To be added.</summary>
        /// <param name="sender">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.OnDeserialization">`Dictionary.OnDeserialization` on docs.go-mono.com</a></footer>
        public virtual void OnDeserialization(object sender)
        {
          _value.OnDeserialization(sender);
        }

        /// <summary><para>Removes the element with the specified key from the dictionary.</para></summary>
        /// <param name="key">The key of the element to be removed from the dictionary.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Remove">`Dictionary.Remove` on docs.go-mono.com</a></footer>
        public bool Remove(TKey key)
        {
          bool output = _value.Remove(key);
          if(output) changed?.Invoke();
          return output;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(
          KeyValuePair<TKey, TValue> keyValuePair)
        {
          ((ICollection<KeyValuePair<TKey, TValue>>)_value).Add(keyValuePair);
          changed?.Invoke();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(
          KeyValuePair<TKey, TValue> keyValuePair)
        {
          return ((ICollection<KeyValuePair<TKey, TValue>>)_value).Contains(keyValuePair);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
          KeyValuePair<TKey, TValue>[] array,
          int index)
        {
          ((ICollection<KeyValuePair<TKey, TValue>>)_value).CopyTo(array, index);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
          KeyValuePair<TKey, TValue> keyValuePair)
        {
          bool output = ((ICollection<KeyValuePair<TKey, TValue>>)_value).Remove(keyValuePair);
          if(output) changed?.Invoke();
          return output;
        }

        /// <summary><para>Returns an enumerator that can be used to iterate over the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.GetEnumerator">`Dictionary.GetEnumerator` on docs.go-mono.com</a></footer>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
          return _value.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
          ((ICollection)_value).CopyTo(array, index);
        }

        /// <summary><para>Adds an element with the specified key and value to the dictionary.</para></summary>
        /// <param name="key">The key of the element to add to the dictionary.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Add">`Dictionary.Add` on docs.go-mono.com</a></footer>
        void IDictionary.Add(object key, object value)
        {
          ((IDictionary)_value).Add(key, value);
          changed?.Invoke();
        }

        bool IDictionary.Contains(object key)
        {
          return ((IDictionary)_value).Contains(key);
        }

        /// <summary><para>Returns an enumerator that can be used to iterate over the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.GetEnumerator">`Dictionary.GetEnumerator` on docs.go-mono.com</a></footer>
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
          return ((IDictionary)_value).GetEnumerator();
        }

        /// <summary><para>Removes the element with the specified key from the dictionary.</para></summary>
        /// <param name="key">The key of the element to be removed from the dictionary.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.Remove">`Dictionary.Remove` on docs.go-mono.com</a></footer>
        void IDictionary.Remove(object key)
        {
          ((IDictionary)_value).Remove(key); 
          changed?.Invoke();
        }

        /// <summary><para>Returns an enumerator that can be used to iterate over the dictionary.</para></summary>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.GetEnumerator">`Dictionary.GetEnumerator` on docs.go-mono.com</a></footer>
        IEnumerator IEnumerable.GetEnumerator()
        {
          return ((IEnumerable)_value).GetEnumerator();
        }

        /// <summary>To be added.</summary>
        /// <param name="key">To be added.</param>
        /// <footer><a href="http://docs.go-mono.com/?link=M:System.Collections.Generic.Dictionary%3CTKey,TValue%3E.TryGetValue">`Dictionary.TryGetValue` on docs.go-mono.com</a></footer>
        public bool TryGetValue(TKey key, out TValue value)
        {
          return _value.TryGetValue(key, out value);
        }
    }
}
