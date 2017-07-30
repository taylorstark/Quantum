using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Spark.Cache;
using Spark.Cache.Filter;
using Xunit;

namespace Spark.Test.Cache.Filter
{
    public class AsyncCacheFilterTests
    {
        private class TestAsyncCacheFilter : AsyncCacheFilter<int, int>
        {
            public override ValueTask<FilterAction> PreAddAsync(int key, int value)
            {
                throw new NotSupportedException();
            }

            public override Task PostAddAsync(FilterResult result, int key, int value)
            {
                throw new NotSupportedException();
            }

            public override ValueTask<FilterAction> PreGetAsync(int key)
            {
                throw new NotSupportedException();
            }

            public override Task PostGetAsync(FilterResult result, int key, bool exists, int value)
            {
                throw new NotSupportedException();
            }

            public override ValueTask<FilterAction> PreRemoveAsync(int key)
            {
                throw new NotSupportedException();
            }

            public override Task PostRemoveAsync(FilterResult result, int key, bool existed)
            {
                throw new NotSupportedException();
            }

            public new Task AddAsync(int key, int value)
            {
                return base.AddAsync(key, value);
            }

            public new ValueTask<uint> CountAsync()
            {
                return base.CountAsync();
            }

            public new ValueTask<bool> ContainsAsync(int key)
            {
                return base.ContainsAsync(key);
            }

            public new ValueTask<(bool exists, int value)> GetAsync(int key)
            {
                return base.GetAsync(key);
            }

            public new ValueTask<bool> IsEmptyAsync()
            {
                return base.IsEmptyAsync();
            }

            public new ValueTask<ICollection<int>> KeysAsync()
            {
                return base.KeysAsync();
            }

            public new ValueTask<bool> RemoveAsync(int key)
            {
                return base.RemoveAsync(key);
            }
        }

        [Fact]
        public void NotAttachedByDefault()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            Assert.False(cacheFilter.IsAttached);
        }

        [Fact]
        public void Attach()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);

            Assert.True(cacheFilter.IsAttached);
        }

        [Fact]
        public void AttachWhileAlreadyAttached()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);

            Assert.Throws<FilterAlreadyAttachedException>(() => cacheFilter.Attach(cache));
        }

        [Fact]
        public void Detach()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            cacheFilter.Detach();

            Assert.False(cacheFilter.IsAttached);
        }

        [Fact]
        public void DetachWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            Assert.Throws<FilterNotAttachedException>(() => cacheFilter.Detach());
        }

        [Fact]
        public async Task AddAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            await cacheFilter.AddAsync(0, 1);

            await cache.Received().AddAsync(Arg.Is(0), Arg.Is(1));
        }

        [Fact]
        public async Task AddAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.AddAsync(0, 1));
        }

        [Fact]
        public async Task CountAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.CountAsync().Returns(new ValueTask<uint>(5U));
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            Assert.Equal(5U, await cacheFilter.CountAsync());
        }

        [Fact]
        public async Task CountAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.CountAsync().AsTask());
        }

        [Fact]
        public async Task ContainsAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.ContainsAsync(Arg.Any<int>()).Returns(new ValueTask<bool>(true));
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            Assert.True(await cacheFilter.ContainsAsync(0));
        }

        [Fact]
        public async Task ContainsAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.ContainsAsync(0).AsTask());
        }

        [Fact]
        public async Task GetAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.GetAsync(Arg.Any<int>()).Returns(new ValueTask<(bool, int)>(ValueTuple.Create(true, 1)));
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            (bool exists, int value) = await cacheFilter.GetAsync(0);

            Assert.True(exists);
            Assert.Equal(1, value);
        }

        [Fact]
        public async Task GetAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.GetAsync(0).AsTask());
        }

        [Fact]
        public async Task IsEmptyAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.IsEmptyAsync().Returns(new ValueTask<bool>(true));
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            Assert.True(await cacheFilter.IsEmptyAsync());
        }

        [Fact]
        public async Task IsEmptyAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.IsEmptyAsync().AsTask());
        }

        [Fact]
        public async Task KeysAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.KeysAsync().Returns(new ValueTask<ICollection<int>>(new List<int> { 1, 2 }));
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            Assert.Equal(new List<int> { 1, 2 }, await cacheFilter.KeysAsync());
        }

        [Fact]
        public async Task KeysAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.KeysAsync().AsTask());
        }

        [Fact]
        public async Task RemoveAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.RemoveAsync(Arg.Any<int>()).Returns(new ValueTask<bool>(true));
            var cacheFilter = new TestAsyncCacheFilter();

            cacheFilter.Attach(cache);
            Assert.True(await cacheFilter.RemoveAsync(0));
        }

        [Fact]
        public async Task RemoveAsyncWhileNotAttached()
        {
            var cacheFilter = new TestAsyncCacheFilter();

            await Assert.ThrowsAsync<FilterNotAttachedException>(() => cacheFilter.RemoveAsync(0).AsTask());
        }
    }
}