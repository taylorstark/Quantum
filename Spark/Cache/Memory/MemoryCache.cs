using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Spark.Cache.Memory
{
    public class MemoryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, TValue> Cache { get; }

        public uint Count => (uint)Cache.Count;
        public bool IsEmpty => Cache.IsEmpty;
        public ICollection<TKey> Keys => Cache.Keys;
        public ICollection<TValue> Values => Cache.Values;

        public MemoryCache()
        {
            Cache = new ConcurrentDictionary<TKey, TValue>();
        }

        public MemoryCache(IEqualityComparer<TKey> comparer)
        {
            Cache = new ConcurrentDictionary<TKey, TValue>(comparer);
        }

        public MemoryCache(int concurrencyLevel, int capacity)
        {
            Cache = new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, capacity);
        }

        public MemoryCache(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
        {
            Cache = new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, capacity, comparer);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            Cache[key] = value;
        }

        public bool Contains(TKey key)
        {
            return Cache.ContainsKey(key);
        }

        public bool Get(TKey key, out TValue value)
        {
            return Cache.TryGetValue(key, out value);
        }

        public bool Remove(TKey key)
        {
            return Cache.TryRemove(key, out TValue _);
        }
    }
}
