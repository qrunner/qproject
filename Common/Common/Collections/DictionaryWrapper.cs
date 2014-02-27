using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Collections
{
    public class DictionaryWrapper : IDictionary<string, object>
    {
        private readonly IDictionary<string, string> _source;

        public DictionaryWrapper(IDictionary<string, string> source)
        {
            _source = source;
        }

        public void Add(string key, object value)
        {
            _source.Add(key, Convert.ToString(value));
        }

        public bool ContainsKey(string key)
        {
            return _source.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _source.Keys; }
        }

        public bool Remove(string key)
        {
            return _source.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            string outVal;
            var retval = _source.TryGetValue(key, out outVal);
            value = outVal;
            return retval;
        }

        public ICollection<object> Values
        {
            get { return new[] {new List<string>(_source.Values.ToList())}; }
        }

        public object this[string key]
        {
            get { return _source[key]; }
            set { _source[key] = Convert.ToString(value); }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, Convert.ToString(item.Value));
        }

        public void Clear()
        {
            _source.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _source.Count; }
        }

        public bool IsReadOnly
        {
            get { return _source.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _source.GetEnumerator();
        }
    }
}
