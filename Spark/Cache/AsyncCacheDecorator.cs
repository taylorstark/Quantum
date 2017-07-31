using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spark.Cache
{
    public abstract class AsyncCacheDecorator<TKey, TValue> : IAsyncCache<TKey, TValue>
    {
        private IAsyncCache<TKey, TValue> Cache { get; }

        protected AsyncCacheDecorator(IAsyncCache<TKey, TValue> cache)
        {
            Cache = cache;
        }

        public virtual Task AddAsync(TKey key, TValue value)
        {
            return Cache.AddAsync(key, value);
        }

        public ValueTask<uint> CountAsync()
        {
            return Cache.CountAsync();
        }

        public ValueTask<bool> ContainsAsync(TKey key)
        {
            return Cache.ContainsAsync(key);
        }

        public virtual ValueTask<(bool exists, TValue value)> GetAsync(TKey key)
        {
            return Cache.GetAsync(key);
        }

        public ValueTask<bool> IsEmptyAsync()
        {
            return Cache.IsEmptyAsync();
        }

        public ValueTask<ICollection<TKey>> KeysAsync()
        {
            return Cache.KeysAsync();
        }

        public virtual ValueTask<bool> RemoveAsync(TKey key)
        {
            return Cache.RemoveAsync(key);
        }
    }
}
