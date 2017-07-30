using System.Collections.Generic;
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
    }
}
