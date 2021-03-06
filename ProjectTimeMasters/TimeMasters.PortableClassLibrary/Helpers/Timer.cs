﻿using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;

namespace TimeMasters.PortableClassLibrary.Helpers
{
    internal delegate void TimerCallback(object state);
    internal sealed class Timer : CancellationTokenSource, IDisposable
    {
        internal Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            Contract.Assert(period.Minutes == -1, "This stub implementation only supports dueTime.");
            Task.Delay(dueTime, Token).ContinueWith((t, s) =>
            {
                var tuple = (Tuple<TimerCallback, object>) s;
                tuple.Item1(tuple.Item2);
            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public new void Dispose() { base.Cancel(); }
    }
}