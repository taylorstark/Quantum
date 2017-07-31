using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spark.Cache.Filter
{
    public class RundownProtection
    {
        private const int ExitMask = (1 << 31);

        private int _state;
        private SemaphoreSlim _exitSemaphore;

        public bool Acquire(out AcquiredRundownProtection acquiredRundownProtection)
        {
            // Initialize the out variable.
            acquiredRundownProtection = new AcquiredRundownProtection();

            // Try to acquire rundown protection.
            int currentState;
            int oldState = _state;

            do
            {
                currentState = oldState;

                // Check if we are exiting.
                if ((currentState & ExitMask) == ExitMask)
                {
                    // Rundown protection could not be acquired.
                    acquiredRundownProtection.Disposed = true;
                    return false;
                }

                // Check if we are about to cause an overflow and set the exit mask bit.
                if (((currentState + 1) & ExitMask) == ExitMask)
                {
                    throw new OverflowException();
                }

                oldState = Interlocked.CompareExchange(ref _state,
                                                       currentState + 1,
                                                       currentState);
            } while (oldState != currentState);

            // Rundown protection was acquired successfully.
            acquiredRundownProtection.RundownProtection = this;
            return true;
        }

        internal void Release()
        {
            // Decrement the number of threads that have rundown protection.
            int currentState;
            int newState;
            int oldState = _state;

            do
            {
                currentState = oldState;
                newState = ((currentState & ~ExitMask) - 1) | (currentState & ExitMask);
                oldState = Interlocked.CompareExchange(ref _state,
                                                       newState,
                                                       currentState);
            } while (oldState != currentState);

            // Check if we are exiting and we are the last thread with rundown protection.
            if (newState == ExitMask)
            {
                _exitSemaphore.Release();
            }
        }

        public Task WaitAsync()
        {
            // Signal to any other threads that we are exiting.
            _exitSemaphore = new SemaphoreSlim(0);

            int currentState;
            int oldState = _state;

            do
            {
                currentState = oldState;
                oldState = Interlocked.CompareExchange(ref _state,
                                                       currentState | ExitMask,
                                                       currentState);
            } while (oldState != currentState);

            // If the reference count is 0, then no threads had rundown protection.
            // Otherwise, another thread currently has rundown protection.
            return currentState == 0 ? Task.CompletedTask : _exitSemaphore.WaitAsync();
        }
    }

    public struct AcquiredRundownProtection : IDisposable
    {
        internal bool Disposed { get; set; }
        internal RundownProtection RundownProtection { get; set; }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            RundownProtection.Release();
            Disposed = true;
        }
    }
}