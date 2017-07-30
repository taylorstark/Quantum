using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spark.Cache
{
    public class AsyncCacheAdapter<TKey, TValue> : IAsyncCache<TKey, TValue>
    {
        private ICache<TKey, TValue> Cache { get; }

        public AsyncCacheAdapter(ICache<TKey, TValue> cache)
        {
            Cache = cache;
        }

        public Task AddAsync(TKey key, TValue value)
        {
            Cache.Add(key, value);
            return Task.CompletedTask;
        }

        public ValueTask<uint> CountAsync()
        {
            return new ValueTask<uint>(Cache.Count);
        }

        public ValueTask<bool> ContainsAsync(TKey key)
        {
            return new ValueTask<bool>(Cache.Contains(key));
        }

        public ValueTask<(bool exists, TValue value)> GetAsync(TKey key)
        {
            var exists = Cache.Get(key, out TValue value);
            return new ValueTask<(bool, TValue)>(ValueTuple.Create(exists, value));
        }

        public ValueTask<bool> IsEmptyAsync()
        {
            return new ValueTask<bool>(Cache.IsEmpty);
        }

        public ValueTask<ICollection<TKey>> KeysAsync()
        {
            return new ValueTask<ICollection<TKey>>(Cache.Keys);
        }

        public ValueTask<bool> RemoveAsync(TKey key)
        {
            return new ValueTask<bool>(Cache.Remove(key));
        }
    }
}
