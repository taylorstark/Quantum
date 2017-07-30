using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Spark.Cache.Filter
{
    public class FilterAlreadyAttachedException : Exception { }
    public class FilterNotAttachedException : Exception { }

    public abstract class AsyncCacheFilter<TKey, TValue>
    {
        // This lock is used to prevent against simultaneous attempts to add
        // this filter to multiple filtered caches.
        private object AttachLock { get; } = new object();

        private IAsyncCache<TKey, TValue> Cache { get; set; }

        // This field is volatile to prevent reordering of reads between
        // Cache and IsAttached. Otherwise, we could end up with either:
        //   - IsAttached = true, Cache = null
        //   - IsAttached = false, Cache = non null
        private volatile bool _attached;
        public bool IsAttached
        {
            get => _attached;
            private set => _attached = value;
        }

        public abstract ValueTask<FilterAction> PreAddAsync(TKey key, TValue value);
        public abstract Task PostAddAsync(FilterResult result, TKey key, TValue value);
        public abstract ValueTask<FilterAction> PreGetAsync(TKey key);
        public abstract Task PostGetAsync(FilterResult result, TKey key, bool exists, TValue value);
        public abstract ValueTask<FilterAction> PreRemoveAsync(TKey key);
        public abstract Task PostRemoveAsync(FilterResult result, TKey key, bool existed);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfNotAttached()
        {
            if (!IsAttached)
            {
                throw new FilterNotAttachedException();
            }
        }

        internal void Attach(IAsyncCache<TKey, TValue> cache)
        {
            lock (AttachLock)
            {
                if (IsAttached)
                {
                    throw new FilterAlreadyAttachedException();
                }

                Cache = cache;
                IsAttached = true;
            }
        }

        internal void Detach()
        {
            ThrowIfNotAttached();
            IsAttached = false;
            Cache = null;
        }

        protected Task AddAsync(TKey key, TValue value)
        {
            ThrowIfNotAttached();
            return Cache.AddAsync(key, value);
        }

        protected ValueTask<uint> CountAsync()
        {
            ThrowIfNotAttached();
            return Cache.CountAsync();
        }

        protected ValueTask<bool> ContainsAsync(TKey key)
        {
            ThrowIfNotAttached();
            return Cache.ContainsAsync(key);
        }

        protected ValueTask<(bool, TValue)> GetAsync(TKey key)
        {
            ThrowIfNotAttached();
            return Cache.GetAsync(key);
        }

        protected ValueTask<bool> IsEmptyAsync()
        {
            ThrowIfNotAttached();
            return Cache.IsEmptyAsync();
        }

        protected ValueTask<ICollection<TKey>> KeysAsync()
        {
            ThrowIfNotAttached();
            return Cache.KeysAsync();
        }

        protected ValueTask<bool> RemoveAsync(TKey key)
        {
            ThrowIfNotAttached();
            return Cache.RemoveAsync(key);
        }
    }
}