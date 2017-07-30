using System.Collections.Generic;

namespace Spark.Cache
{
    public interface ICache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        uint Count { get; }
        bool IsEmpty { get; }
        ICollection<TKey> Keys { get; }
        ICollection<TValue> Values { get; }

        void Add(TKey key, TValue value);
        bool Contains(TKey key);
        bool Get(TKey key, out TValue value);
        bool Remove(TKey key);
    }
}
