using StressClient.Core.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

namespace StressClient.Core
{
    public class StressRun : IRun
    {
        public Stopwatch Timer;
        public List<IVirtualUser> VUsers;
        private int DurationInMs;
        private EventWaitHandle CancelWaitHandle;

        public bool IsRunning { get { return this.Timer != null && this.Timer.IsRunning; } }
        public TimeSpan Elapsed { get { return this.IsRunning ? this.Timer.Elapsed : TimeSpan.Zero; } }

        private StressRun()
        {

        }

        public void Run()
        {
            if (!IsRunning)
            {
                Timer = Stopwatch.StartNew();
                bool signaled = false;

                do
                {
                    signaled = this.CancelWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                    // ToDo: Something else if desired.
                } while (!signaled && this.Timer.ElapsedMilliseconds < this.DurationInMs);

                this.Stop();
            }
        }

        public void Cancel()
        {
            this.CancelWaitHandle.Set();
        }

        private void Stop()
        {
            this.Timer.Stop();

            Trace.WriteLine(string.Format("{0}: Test finished (elapsed {1})", DateTime.Now, this.Timer.Elapsed));

            VUsers.ForEach(c => { c.Stop(); });
        }


        public static StressRun Setup<T>(int numberOfUsers, int durationInMs, bool constantLoad) where T : IVirtualUser, new()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;

            StressRun run = new StressRun();
            run.DurationInMs = durationInMs;

            //step 0 : create visual users
            run.VUsers = new List<IVirtualUser>(numberOfUsers);
            Enumerable.Range(1, numberOfUsers).ToList().ForEach(i => run.VUsers.Add(new T()));

            //step 1 : setup all virtual users
            run.VUsers.ForEach(async c => { 
                await c.Setup(run); 
            });

            //dynmaic user load not implemented yet
            //if (percentageNewUsers != double.NaN)
            //{
            //    run.TimerNewUsers = new Timer(state =>
            //    {

            //    }, run.Timer.Elapsed.TotalSeconds, Timeout.Infinite, 30 * 1000);
            //}

            run.CancelWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            return run;
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
