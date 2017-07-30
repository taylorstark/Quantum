using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Spark.Cache;
using Xunit;

namespace Spark.Test.Cache
{
    public class AsyncCacheDecoratorTests
    {
        private class TestAsyncCacheDecorator<TKey, TValue> : AsyncCacheDecorator<TKey, TValue>
        {
            public TestAsyncCacheDecorator(IAsyncCache<TKey, TValue> cache) : base(cache)
            {
            }
        }

        [Fact]
        public async Task ProxyAddAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);

            await decorator.AddAsync(0, 1);

            await cache.Received().AddAsync(Arg.Is(0), Arg.Is(1));
        }

        [Fact]
        public async Task ProxyCountAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.CountAsync().Returns(new ValueTask<uint>(10U));
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);

            Assert.Equal(10U, await decorator.CountAsync());
        }

        [Fact]
        public async Task ProxyContainsAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.ContainsAsync(Arg.Any<int>()).Returns(new ValueTask<bool>(true));
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);

            Assert.True(await decorator.ContainsAsync(0));
        }

        [Fact]
        public async Task ProxyGetAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.GetAsync(Arg.Any<int>()).Returns(new ValueTask<(bool, int)>(ValueTuple.Create(true, 1)));
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);

            (bool exists, int value) = await decorator.GetAsync(0);

            Assert.True(exists);
            Assert.Equal(1, value);
        }

        [Fact]
        public async Task ProxyGetKeysAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.KeysAsync().Returns(new ValueTask<ICollection<int>>(new List<int> { 1, 2 }));
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);
            
            Assert.Equal(new List<int> { 1, 2 }, await decorator.KeysAsync());
        }

        [Fact]
        public async Task ProxyIsEmptyAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.IsEmptyAsync().Returns(new ValueTask<bool>(false));
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);

            Assert.False(await decorator.IsEmptyAsync());
        }

        [Fact]
        public async Task ProxyRemoveAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.RemoveAsync(Arg.Any<int>()).Returns(new ValueTask<bool>(true));
            var decorator = new TestAsyncCacheDecorator<int, int>(cache);

            Assert.True(await decorator.RemoveAsync(0));
        }
    }
}
