using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Spark.Cache;
using Xunit;

namespace Spark.Test.Cache
{
    public class CacheExtensionsTests
    {
        [Fact]
        public void ClearEmptyCache()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Keys.Returns(new List<int>());
            cache.Remove(Arg.Any<int>()).Returns(true);

            cache.Clear();

            cache.DidNotReceive().Remove(Arg.Any<int>());
        }

        [Fact]
        public void ClearCache()
        {
            var cache = Substitute.For<ICache<int, int>>();
            cache.Keys.Returns(new List<int> { 1, 2 });
            cache.Remove(Arg.Any<int>()).Returns(true);

            cache.Clear();

            cache.Received().Remove(Arg.Is(1));
            cache.Received().Remove(Arg.Is(2));
        }

        [Fact]
        public async Task ClearEmptyCacheAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.KeysAsync().Returns(new ValueTask<ICollection<int>>(new List<int>()));
            cache.RemoveAsync(Arg.Any<int>()).Returns(new ValueTask<bool>(true));

            await cache.Clear();

            await cache.DidNotReceive().RemoveAsync(Arg.Any<int>());
        }

        [Fact]
        public async Task ClearCacheAsync()
        {
            var cache = Substitute.For<IAsyncCache<int, int>>();
            cache.KeysAsync().Returns(new ValueTask<ICollection<int>>(new List<int> { 1, 2 }));
            cache.RemoveAsync(Arg.Any<int>()).Returns(new ValueTask<bool>(true));

            await cache.Clear();

            await cache.Received().RemoveAsync(Arg.Is(1));
            await cache.Received().RemoveAsync(Arg.Is(2));
        }
    }
}
