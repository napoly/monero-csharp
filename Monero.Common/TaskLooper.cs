using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monero.Common
{
    public class TaskLooper
    {
        private readonly Action _task;
        private long _periodInMs;
        private CancellationTokenSource _cts;
        private Task _loopTask;
        private readonly object _lock = new object();

        public TaskLooper(Action task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public Action GetTask() => _task;

        public bool IsStarted
        {
            get
            {
                lock (_lock)
                {
                    return _cts != null && !_cts.IsCancellationRequested;
                }
            }
        }

        public TaskLooper Start(long periodInMs, bool targetFixedPeriod = false)
        {
            if (periodInMs <= 0)
                throw new ArgumentException("Period must be greater than 0 ms", nameof(periodInMs));

            lock (_lock)
            {
                if (IsStarted) return this;

                _periodInMs = periodInMs;
                _cts = new CancellationTokenSource();

                _loopTask = Task.Run(async () =>
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        var startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        try
                        {
                            _task();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Task execution error: {ex}");
                        }

                        if (_cts.Token.IsCancellationRequested) break;

                        long delay = _periodInMs;
                        if (targetFixedPeriod)
                        {
                            var elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime;
                            delay = Math.Max(0, _periodInMs - elapsed);
                        }

                        try
                        {
                            await Task.Delay((int)delay, _cts.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                    }
                }, _cts.Token);
            }

            return this;
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (_cts != null && !_cts.IsCancellationRequested)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = null;
                }
            }
        }

        public void SetPeriodInMs(long periodInMs)
        {
            if (periodInMs <= 0)
                throw new ArgumentException("Period must be greater than 0 ms");

            lock (_lock)
            {
                _periodInMs = periodInMs;
            }
        }
    }
}
