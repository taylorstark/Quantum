using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Spark.Cache;
using Xunit;

namespace Spark.Test.Cache
{
    public class AsyncCacheAdapterTests
    {
        [Fact]
        public async Task ProxyAddAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            await adapter.AddAsync(0, 1);

            cache.Received().Add(Arg.Is(0), Arg.Is(1));
        }

        [Fact]
        public async Task ProxyContainsAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Contains(Arg.Any<int>()).Returns(true);
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            Assert.True(await adapter.ContainsAsync(0));
        }

        [Fact]
        public async Task ProxyCountAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Count.Returns(2U);
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            Assert.Equal(2U, await adapter.CountAsync());
        }

        [Fact]
        public async Task ProxyGetAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Get(Arg.Any<int>(), out _).Returns(x =>
            {
                x[1] = 1;
                return true;
            });
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            (bool exists, int value) = await adapter.GetAsync(0);

            Assert.True(exists);
            Assert.Equal(1, value);
        }

        [Fact]
        public async Task ProxyGetKeysAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Keys.Returns(new List<int> { 1, 2 });
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            Assert.Equal(new List<int> { 1, 2 }, await adapter.KeysAsync());
        }

        [Fact]
        public async Task ProxyIsEmptyAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.IsEmpty.Returns(false);
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            Assert.False(await adapter.IsEmptyAsync());
        }

        [Fact]
        public async Task ProxyRemoveAsync()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Remove(Arg.Any<int>()).Returns(true);
            var adapter = new AsyncCacheAdapter<int, int>(cache);

            Assert.True(await adapter.RemoveAsync(0));
        }
    }
}
