using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Common.Configuration
{
    /// <summary>
    /// Предоставляет возможность обращенения к KeyValueConfigurationCollection как к IDictionary<string, string>
    /// </summary>
    public class KeyValueConfigurationCollectionWrapper : IDictionary<string, string>
    {
        private readonly KeyValueConfigurationCollection _collection;

        public KeyValueConfigurationCollectionWrapper(KeyValueConfigurationCollection configurationCollection)
        {
            _collection = configurationCollection;
        }

        public void Add(string key, string value)
        {
            _collection.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _collection.AllKeys.Contains(key);
        }

        public ICollection<string> Keys
        {
            get { return _collection.AllKeys; }
        }

        public bool Remove(string key)
        {
            if (!ContainsKey(key))
                return false;

            _collection.Remove(key);
            return true;
        }

        public bool TryGetValue(string key, out string value)
        {
            value = null;
            if (!ContainsKey(key))
                return false;

            value = _collection[key].Value;
            return true;
        }

        public ICollection<string> Values
        {
            get { return new List<string>(_collection.AllKeys.Select(key => _collection[key].Value)); }
        }

        public string this[string key]
        {
            get { return _collection[key].Value; }
            set
            {
                if (ContainsKey(key))
                    _collection[key].Value = value;
                else
                    Add(key, value);
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ContainsKey(item.Key) && _collection[item.Key].Value == item.Value;
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}