using System.Threading.Tasks;
using Spark.Cache.Filter;
using Xunit;

namespace Spark.Test.Cache.Filter
{
    public class RundownProtectionTests
    {
        [Fact]
        public async Task AcquireThenReleaseThenWait()
        {
            var rundownProtection = new RundownProtection();

            var acquired = rundownProtection.Acquire(out AcquiredRundownProtection acquiredRundownProtection);
            Assert.True(acquired);
            Assert.False(acquiredRundownProtection.Disposed);

            acquiredRundownProtection.Dispose();
            Assert.True(acquiredRundownProtection.Disposed);

            await rundownProtection.WaitAsync();
        }

        [Fact]
        public async Task AcquireThenWaitThenRelease()
        {
            var rundownProtection = new RundownProtection();

            var acquired = rundownProtection.Acquire(out AcquiredRundownProtection acquiredRundownProtection);
            Assert.True(acquired);
            Assert.False(acquiredRundownProtection.Disposed);

            var rundownProtectionWaitTask = rundownProtection.WaitAsync();
            Assert.False(acquiredRundownProtection.Disposed);

            acquiredRundownProtection.Dispose();
            Assert.True(acquiredRundownProtection.Disposed);

            await rundownProtectionWaitTask;
        }

        [Fact]
        public async Task WaitThenAcquire()
        {
            var rundownProtection = new RundownProtection();

            await rundownProtection.WaitAsync();

            Assert.False(rundownProtection.Acquire(out AcquiredRundownProtection _));
        }
    }
}