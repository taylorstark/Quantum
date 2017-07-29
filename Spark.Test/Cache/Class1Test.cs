using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace Spark.Test.Cache
{
    public class Class1Test
    {
        [Fact]
        public void Test()
        {
            var test = Substitute.For<IList<object>>();
            test.Contains(Arg.Any<object>()).Returns(true);
            Assert.True(test.Contains(new object()));
            Assert.Equal("Hi", new Class1().Blah());
        }
    }
}
