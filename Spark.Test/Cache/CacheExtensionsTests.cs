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
            var cache = Substitute.For<ICache<uint, uint>>();
            cache.Keys.Returns(new List<uint>());
            cache.Remove(Arg.Any<uint>()).Returns(true);

            cache.Clear();

            cache.DidNotReceive().Remove(Arg.Any<uint>());
        }

        [Fact]
        public void ClearCache()
        {
            var cache = Substitute.For<ICache<uint, uint>>();
            cache.Keys.Returns(new List<uint> { 1, 2 });
            cache.Remove(Arg.Any<uint>()).Returns(true);

            cache.Clear();

            cache.Received().Remove(Arg.Is(1U));
            cache.Received().Remove(Arg.Is(2U));
        }

        [Fact]
        public async Task ClearEmptyCacheAsync()
        {
            var cache = Substitute.For<IAsyncCache<uint, uint>>();
            cache.Keys.Returns(new ValueTask<ICollection<uint>>(new List<uint>()));
            cache.Remove(Arg.Any<uint>()).Returns(new ValueTask<bool>(true));

            await cache.Clear();

            await cache.DidNotReceive().Remove(Arg.Any<uint>());
        }

        [Fact]
        public async Task ClearCacheAsync()
        {
            var cache = Substitute.For<IAsyncCache<uint, uint>>();
            cache.Keys.Returns(new ValueTask<ICollection<uint>>(new List<uint> { 1, 2 }));
            cache.Remove(Arg.Any<uint>()).Returns(new ValueTask<bool>(true));

            await cache.Clear();

            await cache.Received().Remove(Arg.Is(1U));
            await cache.Received().Remove(Arg.Is(2U));
        }
    }
}
